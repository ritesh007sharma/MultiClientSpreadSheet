/**
 * holds main, parses JSON from NetController.receive
 * and forwards the cell info to spreadsheet manager.
 * associated with the proper clientID
 * or file info to file manager
 * receives messages from spreadsheet manager
 * to send back to clients appropriate clients.
 */

#include "NetController.h"
#include <iostream>
#include <dirent.h>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include "JSONEncoder.h"
#include <sstream>
#include <fstream>
#include <cstdio>
#include <signal.h>
#include "spreadsheet.h"

#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"

using namespace std;

// Returns a list of all files in the sheets directory
set<string>* getFileList();

set<string>* fileList;

// The currently active sheets, the sheets that have been opened during execution
map<string, Spreadsheet*> activeSheets;

// Data structure for storing user information
// key: username, value: password
map<string, string> users;

NetController net; // Network interface

ConnectionBundle adminConnection;

bool shutdown;

void clientConnected(ConnectionBundle*);

// Searches the active connections for all sockets associated with the sheet specified in bundle.
// Sends the provided message to all such clients
void sendMessageToOtherConnections(ConnectionBundle *bundle, string msg){
        vector<ConnectionBundle> active = net.getCurrentConnectionList();
        for(int i = 0; i < active.size(); i++){
                ConnectionBundle thisBundle = active.data()[i];
                if(thisBundle.sheet == bundle->sheet){ // If connection is associated with the proper sheet
                        net.sendData(msg, thisBundle.state->getId()); // Send the data out.
                }
        }
}

/**
 * Load a spreadsheet from filename
 */
string load(string filename)
{

  string line;
  string wholefile;
 
  ifstream myfile (filename.c_str());


  bool hasString=false;

  if(myfile.is_open())
  {
    while ( getline (myfile,line) )
    {
      //copying text to string
      wholefile += line;
      hasString=true;

    }
    myfile.close();
  }
  else
  {
    cout << "Unable to open file"<<endl;

    return "";
  }

  if(!hasString)
  {
    cout<<"file was empty"<<endl;

    return "";
  }

  return wholefile;
}



map<string, string> unpackUsers(string message)
{
  if(message.empty()){
    map<string,string> empty;
    return empty;
  }

  rapidjson::Document d;


  d.Parse(message.c_str());

  rapidjson::Value::ConstMemberIterator iter;

  map<string,string> retUsers;

  for(iter=d.MemberBegin();iter<d.MemberEnd();++iter)
  {
    retUsers.insert({iter->value["u"].GetString(),iter->value["p"].GetString()});
  }

  return retUsers;
}


/**
 * Save a sheet out to file
 */
string save(string filename, string message) {

  ofstream myfile (filename.c_str());
  


  //save it
  if(myfile.is_open())
  {
    myfile<< message << endl;
  }
  else
  {
    cout << "Unable to open file"<<endl;

  }

  myfile.close();


  return message;
}

/**
 * Validate username/password. Create a user if it doesn't exit
 */
bool validateUser(string username, string password){
        if(users.find(username) == users.end()){ // User doesn't exist, register
                users.insert({username, password});
                save("users" ,JSONEncoder::packUsers(users));
                cout << "New user created" << endl;
                return true;
        }
        else{ // User exists, validate
                string actual = users[username]; // Fetch password

                // Compare for equality
                if(actual.compare(password) == 0){ // Good password
                        cout << "User exists, password good" << endl;
                        return true;
                }
                else{ // Bad password
                        cout << "User exists, password bad" << endl;
                        return false;
                }
        }
}

// Deletes the user with the associated username from the user vector
void deleteUser(string username){
        users.erase(username);
}

// Removes a file from disk
void removeFromDisk(string fname){
        string fileloc = "rm ./sheets/";
        fileloc.append(fname);
        system(fileloc.c_str());
}

// Deletes the spreadsheet with the associated name from disk
void deleteFile(string fname){
        activeSheets.erase(fname);
        fileList->erase(fname);
        removeFromDisk(fname);


        // Remove file
}

/**
 * Callback for when data is received from the client.
 *
 * TODO: The ConnectionBundle struct is a bit muddled and unnecessary, left in for now
 *  in case it's needed for some other function
 */
void receiver(ConnectionBundle *bundle, string msg){
        cout << "command: " << msg << endl;
        //NetController::getInstance()->sendData(msg, bundle->state->getId());

        // Parse message and respond accordingly
        rapidjson::Document d; // message dictionary
        rapidjson::ParseResult ok = d.Parse(msg.c_str());

        // Reject bad json
        if(!ok){
                cout << "message rejected..." << endl;
                return;
        }

        string type = d["type"].GetString();

        if(type == "open"){ // Open sheet command
                string name = d["name"].GetString();
                string username = d["username"].GetString();
                string password = d["password"].GetString();

                // Check username password
                //  For now, just let it happen
                bool userValid = validateUser(username, password);
                if(!userValid){

                        string error = JSONEncoder::packError(1, "");
                        net.sendData(error, bundle->state->getId());
                        return;
                }
                

                // Search the file list vector, to see if the sheet exists
                // bool fileExists = false;
                // if(find(fileList->begin(), fileList->end(), name) != fileList->end()){
                //         fileExists = true;
                // }

                

                // Initilize new sheet object
                //    - New if not in sheets directory
                //    - From file if it is
    
                Spreadsheet *sheet;
                
                if(activeSheets.find(name) == activeSheets.end()){ // Sheet isn't active yet
                        sheet = new Spreadsheet(name); // Will open a new file if it doesn't exist on disk
                        fileList->insert(name); // Add new sheet to file list

                        //clientConnected(&adminConnection);


                        //sending users
                        string result = JSONEncoder::packUsers(users);
                        net.sendData(result, bundle->state->getId());


                        
                        cout << "New sheet created" << endl;

                        activeSheets.insert({name, sheet});
                }
                else{ // Sheet is active
                        sheet = activeSheets[name];
                        cout << "Existing sheet loaded" << endl;
                }

                bundle->sheet = sheet; // Associate sheet with connection

                set<string> dep;

                cout << "Opening sheet " << name << " for " << username << endl;
                cout << sheet->getFullSend() << endl;
                net.sendData(sheet->getFullSend(), bundle->state->getId());
        }
        else if(type == "get_users"){ // Kick back a user list, for the admin tool
                string result = JSONEncoder::packUsers(users);
                cout << result << endl;
                net.sendData(result, bundle->state->getId());
                if(adminConnection.sheet == NULL)
                {
                adminConnection = *bundle;
                }
        }
        else if(type == "delete_sheet"){
                string name = d["name"].GetString();
                // Remove from file list, spreadsheet map, and disk
                cout << "Deleting sheet " << name << endl;
                deleteFile(name);
        }
        else if(type == "delete_user"){
                string username = d["username"].GetString();
                cout << "Deleting user " << username << endl;
                deleteUser(username);
        }
        else if(type == "shutdown"){

            

         shutdown = true; 
        }
        

        else if(type=="disconnect")
        {
            
                cout<< bundle->sheet << endl;
                if(bundle->sheet !=0)
                {
                
                        bundle->sheet->getSpreadsheet();
              
                }

        }
        else if(type == "edit"){ // Edit a cell
                string cell = d["cell"].GetString();
                // TODO - Need to decode array of strings to get depencies
                set<string> dep;
                rapidjson::Value depVal = d["dependencies"].GetArray();
                for(rapidjson::SizeType i = 0; i < depVal.Size(); i++){
                        string strVal = string(depVal[i].GetString());
                        cout << "dep: " << strVal << endl;
                        dep.insert(strVal);
                }


                //cout << "Edit received for cell " << cell << " with value " << value << endl;

                if(bundle->sheet != 0){
                        //string value = "";
                        string delta = "";

                        if(d["value"].IsString()){
                                string value = d["value"].GetString();
                                delta = bundle->sheet->edit(cell, value, dep);
                        }
                        else if(d["value"].IsNumber()){
                                if(d["value"].IsInt()){
                                        cout<<"cell is an int" << endl;
                                        int value = d["value"].GetInt();
                                        delta = bundle->sheet->edit(cell, (double)value, dep);
                                }
                                else if(d["value"].IsDouble()){
                                        cout<<"cell is an double" << endl;
                                        double value = d["value"].GetDouble();
                                        delta = bundle->sheet->edit(cell, value, dep);
                                }

                        }

                        string full = bundle->sheet->getFullSend();
                        sendMessageToOtherConnections(bundle, delta);
                        cout << delta << endl;
                }
                else{
                        cout << "Sheet not opened..." << endl;
                }
        }
        else if(type == "undo"){
                cout << "Received undo" << endl;
                if(bundle->sheet != 0){
                        string delta = bundle->sheet->undo();
                        sendMessageToOtherConnections(bundle, delta);
                        cout << delta << endl;
                }
        }
        else if(type == "revert"){
                string cell = d["cell"].GetString();
                cout << "Received revert for cell " << cell << endl;
                if(bundle->sheet != 0){
                        string delta = bundle->sheet->revert(cell);
                        sendMessageToOtherConnections(bundle, delta);
                        cout << delta << endl;
                }
        }
}

void clientConnected(ConnectionBundle *bundle){
        // Kick list back and begin waiting for an open message
        vector<string> vecSet;
        set<string>::iterator it;
        
       // vecSet.push_back("make spreadsheet");

        for(it = fileList->begin(); it != fileList->end(); ++it)
        {
                vecSet.push_back(*it); // TODO - Refactory this to a more meaningful name
        }

        
        
        string json = JSONEncoder::packList(vecSet.data(), vecSet.size());
        cout << json << endl;
        net.sendData(json, bundle->state->getId());
}

void sigHandler(int sig){
        cout << "ctrl-c" << endl;
        shutdown = true;
}

int main(){
        signal(SIGINT, sigHandler);
        shutdown = false;
        fileList = getFileList();
        users=unpackUsers(load("users"));
        net.registerReceiver(receiver);
        net.registerClientReceiver(clientConnected);
        net.beginConnect();

        cout << "Running..." << endl;
        while(!shutdown); // Wait forever...
        cout<< "got after the shutdown" << endl;

        for(auto const& sp : activeSheets){
                sp.second->getSpreadsheet();
                cout << "in loop" << endl;
        }


}

set<string>* getFileList(){
        set<string> *list = new set<string>();

        DIR *pDir;
        struct dirent *entry;
        if(pDir = opendir("./sheets")){
                while(entry = readdir(pDir)){
                        if(strcmp(entry->d_name, ".") != 0 && strcmp(entry->d_name, "..") != 0){ // Exclude hidden files
                             list->insert(entry->d_name);
                             cout << entry->d_name << endl;
                        }
                }
        }

        return list;
}
