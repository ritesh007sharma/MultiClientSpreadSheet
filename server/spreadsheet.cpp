#include <sstream>

#include "spreadsheet.h"
#include <fstream>
#include <iostream>
#include "JSONEncoder.h"
#include <string>
#include <stdlib.h>

#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"



using namespace std;


Spreadsheet::Spreadsheet(string name):spreadSheetName(name) {

  cout<< "spd name :" << name << endl;

  Spreadsheet::loadSpreadsheet(name);


}

Spreadsheet::~Spreadsheet() {
  Spreadsheet::getSpreadsheet();
  while (!history.empty()) {
    Reversion *r = history.back();
    history.pop_back();
    delete r;
  }
}

string Spreadsheet::messageHandler(string message) {

}


/**
 * add a cell with the supplied info long or double
 */
string Spreadsheet::edit(std::string cellID, double contents, std::set<string> dependencies) {

  vector<string> empty;
  if (this->dependencies.hasCircularDependency(cellID, cellID, dependencies, empty)) {
    return getCircularError(cellID);
  }

  if (!cells.count(cellID)) {
    set<string> empty;
    history.push_back(new Reversion(cellID, ContentsVar(""), empty));
    cells.insert(std::make_pair(cellID, ContentsVar(contents)));
  }
  else {
    history.push_back(new Reversion(cellID, cells[cellID], this->dependencies.getDependees(cellID)));
    cells[cellID] = ContentsVar(contents);
  }

  this->dependencies.addDependencies(cellID, dependencies);

  return getDelta(cellID, cells[cellID]);
}

/**
 * add a cell with the supplied info string
 */
string Spreadsheet::edit(string cellID, string contents, set<string> dependencies) {

  vector<string> empty;
  if (this->dependencies.hasCircularDependency(cellID, cellID, dependencies, empty)) {
    return getCircularError(cellID);
  }

  if (!cells.count(cellID)) {
    set<string> empty;
    history.push_back(new Reversion(cellID, ContentsVar(""), empty));
    cells.insert(std::make_pair(cellID, ContentsVar(contents)));
  }
  else {
    history.push_back(new Reversion(cellID, cells[cellID], this->dependencies.getDependees(cellID)));
    cells[cellID] = ContentsVar(contents);
  }

  this->dependencies.addDependencies(cellID, dependencies);

  return getDelta(cellID, cells[cellID]);
}






/**
 * undo spreadsheet
 */
string Spreadsheet::undo() {
  if (!history.empty()) {
    Reversion* r = history.back();
    string cell = r->id;
    history.pop_back();
    cells[r->id] = r->contents;
    dependencies.addDependencies(r->id, r->dependees);

    delete r;

    return getDelta(cell, cells[cell]);
  }

  return "{\n  \"type\": \"full send\",\n  \"spreadsheet\": {}\n}\n\n";
}

/**
 * revert cell
 */
string Spreadsheet::revert(std::string cell) {

  vector<Reversion*> temp;
  Reversion * r = NULL;
  bool circDep = false;

  if (!history.empty()) {
    r = history.back();
    history.pop_back();
    temp.push_back(r);
  }

  while (!history.empty()&&!(r->id == cell&&!r->reverted)) {
    r = history.back();
    history.pop_back();
    temp.push_back(r);
  }


  // If you find a Reversion
  if (r != NULL && r->id == cell && ! r->reverted) {
    vector<string> empty;
    circDep = dependencies.hasCircularDependency(cell, cell, r->dependees, empty);
    if (!circDep) {
      Reversion * r2 = new Reversion(cell, cells[cell], this->dependencies.getDependees(cell));
      r2->reverted=true;
      cells[cell] = r->contents;
      this->dependencies.addDependencies(cell, r->dependees);

     // temp.pop_back();
     // delete r;
     r->reverted = true;

      for (int i = temp.size() - 1; i >= 0; i--) {
        history.push_back(temp[i]);
      }

      history.push_back(r2);
      return getDelta(cell, cells[cell]);
    }
  }
  //If you don't find a Reversion
  else {
    if (r != NULL&& r->reverted)
      for (int i = temp.size() - 1; i >= 0; i--) {
        history.push_back(temp[i]);
      }
    return getDelta(cell, ContentsVar(""));
  }


  // Should only get to this point if there's a circular dependency
  for (int i = temp.size() - 1; i >= 0; i--) {
    history.push_back(temp[i]);
  }
  return getCircularError(cell);

}

/**
 * Get fullsend message for spreadsheet
 */
string Spreadsheet::getFullSend() {
  ostringstream ss;
  ss << "{\n  \"type\": \"full send\",\n  \"spreadsheet\": {\n";

  std::map<string, ContentsVar>::iterator it = cells.begin();

  while (it != cells.end()) {
    if(it->second.type=='s')
      ss << "    \"" << it->first << "\" : \"" << it->second._contents << "\"";
    else
      ss << "    \"" << it->first << "\" : " << it->second._contents;

    it++;

    if (it != cells.end()) {
      ss << ",";
    }
    ss << "\n";
  }

  ss << "  }\n}\n\n";

  return ss.str();
}

string Spreadsheet::getDelta(string cell, ContentsVar contents) {
  if(contents.type=='s')
    return "{\n  \"type\": \"full send\",\n  \"spreadsheet\": {\n    \"" + cell + "\" : \"" + contents._contents + "\"\n  }\n}\n\n";
  else
    return "{\n  \"type\": \"full send\",\n  \"spreadsheet\": {\n    \"" + cell + "\" : " + contents._contents + "\n  }\n}\n\n";
}

string Spreadsheet::getCircularError(string cell) {
  return "{\n  \"type\": \"error\",\n  \"code\": 2,\n  \"source\": \"" + cell + "\"\n}\n\n";
}

/**
 * @brief Load a spreadsheet found at location <code> name</code>
 * Note, this should probably only be done once with the @see{Spreadsheet:Spreadsheet(string)}
 * constructor
 * @param name - the name/directory of the file
 * NOTE, if name and location are supposed to be different, they can be, let me know -Daniel
 */
void Spreadsheet::loadSpreadsheet(string name)
{

  string line;
  string wholefile;
  rapidjson::Document d;
  spreadSheetName = name;
  spreadSheetLoc = "./sheets/" + name;
  cout<< "spd loc is " << spreadSheetLoc << endl;
  
  ifstream myfile (spreadSheetLoc.c_str());


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
    
    Spreadsheet::getSpreadsheet();
    return;
  }

  if(!hasString)
  {
    cout<<"file was empty"<<endl;

    return;
  }

  d.Parse(wholefile.c_str());

  rapidjson::Value::ConstMemberIterator iter;



  for(iter=d[mATTRIBUTES].MemberBegin();iter<d[mATTRIBUTES].MemberEnd();++iter)
  {


    //I'm leaving this line for now because there was a strange bug when desctucting this object without it.. I think
    //I fixed it, but it was really strange so I want to keep this here for now... makes me more comfy

    //cells.insert ( std::pair<string,ContentsVar>(iter->name.GetString(),ContentsVar(iter->value[mCONTENTS].GetString())));

    //load the last contents teh cell had
    if(iter->value[mcONTENTS_TYPE].GetString()[0]=='s')
      cells.insert ( std::pair<string,ContentsVar>(iter->name.GetString(),ContentsVar(iter->value[mCONTENTS].GetString())));
    else if(iter->value[mcONTENTS_TYPE].GetString()[0]=='d')
      cells.insert ( std::pair<string,ContentsVar>(iter->name.GetString(),ContentsVar(strtod(iter->value[mCONTENTS].GetString(),NULL))));
    else if(iter->value[mcONTENTS_TYPE].GetString()[0]=='i')
      cells.insert ( std::pair<string,ContentsVar>(iter->name.GetString(),ContentsVar(strtol(iter->value[mCONTENTS].GetString(),NULL,10))));

    //load the last dependencies the cell had
    set<string> celldependencies;
    for (int i = 0; i < iter->value[mDEPENDENCIES].Size(); i++){
      celldependencies.insert(iter->value[mDEPENDENCIES][i].GetString());

    }
    dependencies.addDependencies(iter->name.GetString(),celldependencies);
  }


  //start getting reversion history
  for(iter =d[mREVERSIONS].MemberBegin(); iter < d[mREVERSIONS].MemberEnd();++iter)
  {
    //get the last dependencies this cell had
    set<string> celldependencies;
    for (int i = 0; i < iter->value[mDEPENDENCIES].Size(); i++){

      celldependencies.insert(iter->value[mDEPENDENCIES][i].GetString());

    }
    //and the last contents it had as well as where it happened (cell id/location)
    Reversion * r2;
    if(iter->value[mcONTENTS_TYPE].GetString()[0]=='s')
      r2=new Reversion(iter->value[mCELL_ID].GetString(), ContentsVar(iter->value[mCONTENTS].GetString()), celldependencies);
    else if(iter->value[mcONTENTS_TYPE].GetString()[0]=='i')
      r2=new Reversion(iter->value[mCELL_ID].GetString(), ContentsVar(strtol(iter->value[mCONTENTS].GetString(),NULL,10)), celldependencies);
    else if(iter->value[mcONTENTS_TYPE].GetString()[0]=='d')
      r2=new Reversion(iter->value[mCELL_ID].GetString(), ContentsVar(strtod(iter->value[mCONTENTS].GetString(),NULL)), celldependencies);

    if(iter->value[mREVERTED].GetString()[0]==mTRUE[0])
    {
      r2->reverted=true;


    }

    //recreate that moment in history
    history.push_back(r2);
  }























}

/**
 * @brief badly named method for saving a spreadsheet
 * note, this method saves the file to the spreadsheet's name, however a second
 * string can be kept with location info if we want to keep save location and name separate
 * @return the JSON string saved to file (you don't jave to do anything with this, it's
 * already saved to a file, it's just provided for extra saves elsewhere or debugging.
 */
string Spreadsheet::getSpreadsheet() {

  if(spreadSheetLoc.empty())
  {
    return "";
  }
  ofstream myfile (spreadSheetLoc.c_str());
  cout << "save spd loc:" <<  spreadSheetLoc << endl;
  
  int size = cells.size();
  cell cellArray[size];
  int counter =0 ;

  //parse a list of cell info for saving
  map<string, ContentsVar> ::iterator it;
  for ( it = cells.begin(); it != cells.end(); it++ )
  {
    cellArray[counter].location = it->first;
    cellArray[counter].contents = it->second._contents;
    cellArray[counter].type = it->second.type;
    counter++;
  }
  //give it all to the JSON encoder for crunching
  string cells = JSONEncoder::packSheet(cellArray, size, spreadSheetName, dependencies, history);//,"ppp");//,spreadSheetName);



  //save it
  if(myfile.is_open())
  {
    myfile<< cells << endl;
  }
  else
  {
    cout << "Unable to open file"<<endl;

  }

  myfile.close();

  return cells;
}

// int main ()
// {
//   Spreadsheet sp("hello");
//   //sp.spreadSheetName = "hello";
//   set<string> setOfCells;
//   setOfCells.insert("A1");
//   setOfCells.insert("B1");
//   cout<<sp.edit("C1",5.6,setOfCells)<<endl;
//   cout<<sp.edit("C3",3,setOfCells)<<endl;
//   cout<<sp.edit("C3","6",setOfCells)<<endl;

//   // cout<<sp.revert("C1")<<endl;
//   //cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   //sp.revert("C1");
//   //sp.undo();
//   //sp.revert("C1");

//   //sp.edit("C4","sdf",setOfCells);
//   //sp.edit("C5","sdfsss",setOfCells);
//   cout << sp.getSpreadsheet() << "\n\n";


//   cout<< "before loading" << endl;

//   Spreadsheet s2;
//   s2.loadSpreadsheet("hello");
//   cout << "got here" << endl;
//   cout << s2.cells["C1"]._contents << endl;
//   cout << s2.cells["C3"]._contents << endl;
//   // cout<< s2.dependencies.getDependees("C1") << endl;
//   set<string> s2d = s2.dependencies.getDependees("C1");

//   std::set<string>:: iterator it;
//   for(it = s2d.begin(); it!= s2d.end(); ++it )
//   {
//     cout << *it << endl;
//   }
//   //cout<< s2.dependencies.getDependees("C3") << endl;

//   vector<Reversion*> reversions = s2.history;

//   std::vector<Reversion*> :: iterator it2;

//   cout<< "before reversions" << endl;

//   for(it2 = reversions.begin(); it2 != reversions.end(); ++ it2)
//   {
//     cout<< (*it2)->id << endl;
//     cout<< (*it2)->contents._contents << endl;
//   }

//   cout<<sp.revert("C1")<<endl;
//   cout<<sp.revert("C1")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C3")<<endl;
//   cout<<sp.revert("C1")<<endl;


//   return 0;
// }
