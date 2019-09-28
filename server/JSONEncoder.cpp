
#include <vector>

#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"

//#include "Cell.h"
#include "JSONEncoder.h"
#include "spreadsheet.h"
#include <vector>
#include<string>
#include <iostream>
#include<ostream>
#include <vector>
#include<stdlib.h>
#include <string>
#include <sstream>



using namespace std;

string JSONEncoder::packError(int code, string source){
  rapidjson::Document document;
  document.SetObject();

  rapidjson::Value array(rapidjson::kArrayType);
  rapidjson::Document::AllocatorType& allocator = document.GetAllocator();

  document.AddMember("type", "error", allocator);
  document.AddMember("code", code, allocator);

  // Need to get a little tricky to encode a string value...
  const char* strVal = source.c_str();
  rapidjson::Value v(rapidjson::StringRef(strVal));
  document.AddMember("source", v, allocator);

  rapidjson::StringBuffer strbuf;
  rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
  document.Accept(writer);

  return string(strbuf.GetString()).append("\n\n");
}

string JSONEncoder::packList(string names[], int length){
  rapidjson::Document document;
  document.SetObject();

  rapidjson::Value array(rapidjson::kArrayType);
  rapidjson::Document::AllocatorType& allocator = document.GetAllocator();

  document.AddMember("type", "list", allocator);


  // Construct a DOM list containing spreadsheet names
  rapidjson::Value nameList(rapidjson::kArrayType);
  for(int i = 0; i < length; i++){
    const char* strVal = names[i].c_str();
    rapidjson::Value val(rapidjson::StringRef(strVal));
    nameList.PushBack(val, allocator);
  }

  document.AddMember("spreadsheets", nameList, allocator);

  rapidjson::StringBuffer strbuf;
  rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
  document.Accept(writer);

  return string(strbuf.GetString()).append("\n\n");
}

string JSONEncoder::packSheet(cell cells[]){
  rapidjson::Document document;
  document.SetObject();

  rapidjson::Value array(rapidjson::kArrayType);
  rapidjson::Document::AllocatorType& allocator = document.GetAllocator();

  document.AddMember("type", "full send", allocator);

  // Construct cell list
  rapidjson::Document sheet(&allocator);
  for(int i = 0; i < sizeof(cells)/sizeof(*cells); i++){
    const char* contents = cells[i].contents.c_str();
    rapidjson::Value val(rapidjson::StringRef(contents));

    const char* location = cells[i].location.c_str();
    rapidjson::Value locval(rapidjson::StringRef(location));
    sheet.AddMember(locval, val, allocator);
  }

  document.AddMember("spreadsheet", sheet, allocator);

  rapidjson::StringBuffer strbuf;
  rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
  document.Accept(writer);

  return strbuf.GetString();
}

/**
 * @brief Add <code>names</code> to <code> toDocument </code> with the category name <code> category.
 * @param category - the name of the category
 * @param names - the list of strings to add under the category
 * @param len - the number of items in the list
 * @param toDocument - the document to add the list to
 * @param alloc - the base allocator for the document (probably best if it's the allocator that persists until
 * the JSON string is completely finished)
 */
void JSONEncoder::addList(string category, string names[], int len, rapidjson::Document& toDocument, rapidjson::Document::AllocatorType& alloc){
  //rapidjson::Document::AllocatorType& alloc = toDocument.GetAllocator();

  //convert the list to a value
  rapidjson::Value nameList(rapidjson::kArrayType);
  for(int i = 0; i < len; i++){
    const char* strVal = names[i].c_str();
    //rapidjson::Value val(rapidjson::StringRef(strVal));
    //val.SetString(strVal, strlen(strVal), alloc);
    //nameList.PushBack(val, alloc);

    nameList.PushBack(rapidjson::Value().SetString(strVal, strlen(strVal), alloc), alloc);
  }

  //convert the category name to a value (with a hard copy)
  const char* cat = category.c_str();
  rapidjson::Value Vcat;
  Vcat.SetString(cat, strlen(cat), alloc);

  //add the set to the document
  toDocument.AddMember(Vcat, nameList, alloc);
}

string JSONEncoder::packUsers(map<string,string> users)
{
  rapidjson::Document document;
  document.SetObject();


  rapidjson::Document::AllocatorType& allocator = document.GetAllocator();
  map<string,string>::iterator it; 

  for(it = users.begin(); it != users.end(); ++it)
  {
    rapidjson::Document node; 
    node.SetObject();
    rapidjson::Value user(rapidjson::StringRef(it->first.c_str()));
    rapidjson::Value pass(rapidjson::StringRef(it->second.c_str()));
    node.AddMember("u",user,allocator);
    node.AddMember("p",pass,allocator);
    document.AddMember("N",node,allocator);
      
  }

  rapidjson::StringBuffer strbuf;
  rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
  document.Accept(writer);

  return string(strbuf.GetString()).append("\n\n");




}

string JSONEncoder::packSheet(cell cells[], int length, string spdName, DependencyGraph dGraph, vector<Reversion*> history){
  rapidjson::Document document;
  document.SetObject();



  //add "name" right now this is the file name, which doesn't have to be included (and isn't really used)
  //however it is here just incase we want to make a more human readable name to pass around
  rapidjson::Document::AllocatorType& allocator = document.GetAllocator();
  rapidjson::Value nameVal(rapidjson::StringRef(spdName.c_str()));
  document.AddMember(mSPREADSHEET_NAME, nameVal, allocator);

  //add a version number for the save file format
  rapidjson::Value vers;    // Null
  vers.SetInt(1);
  document.AddMember(mSAVEFILE_VERSION, vers, allocator);

  rapidjson::Document sheet;
  sheet.SetObject();
  rapidjson::Document::AllocatorType& sheetallocator = document.GetAllocator();
  for(int i = 0; i < length ; i++){
    rapidjson::Document Jcell;
    Jcell.SetObject();


    //add the Cell's contents to the cell as a string
    const char* contents = cells[i].contents.c_str();
    rapidjson::Value val(rapidjson::StringRef(contents));

    //add the contents' type so that we can properly format it later
    const char contType[2]={cells[i].type, '\0'};
    rapidjson::Value valType;
    valType.SetString(contType, 1, sheetallocator);

    Jcell.AddMember(mCONTENTS, val, sheetallocator);
    Jcell.AddMember(mcONTENTS_TYPE,valType,allocator);

    //add the list of dependees to the cell
    set<string> dependees = dGraph.getDependees(cells[i].location);
    set<string> :: iterator it = dependees.begin();
    string dependeeList[dependees.size()];
    int ii=0;
    while(it != dependees.end())
    {
      dependeeList[ii]=*it;
      it++;
      ii++;
    }
    addList(mDEPENDENCIES, dependeeList, dependees.size(), Jcell, sheetallocator);

    //Add the name of the cell as the "object" name for the above contents and
    //dependencies and stick the whole thing inside the spreadsheet attribute object
    const char* cat = cells[i].location.c_str();
    rapidjson::Value Vcat;
    Vcat.SetString(cat, strlen(cat), sheetallocator);
    sheet.AddMember(Vcat, Jcell, sheetallocator);

  }

  //add the cell attribute structure to the base object
  document.AddMember(mATTRIBUTES, sheet, allocator);


  //get the reversion history ready to add to the base object
  rapidjson::Document Jrevis;
  Jrevis.SetObject();

  vector<Reversion*> :: iterator it = history.begin();
  int rcounter =0;
  while(it != history.end())
  {
    rapidjson::Document reversionNode;
    reversionNode.SetObject();

    //get the contents (as a string) and their type
    const char* contents = (*it)->contents._contents.c_str();
    rapidjson::Value val(rapidjson::StringRef(contents));
    const char contType[2]={(*it)->contents.type, '\0'};
    rapidjson::Value valType;
    valType.SetString(contType, 1, sheetallocator);

    //get the id (location) of the cell where teh reversion took place
    const char* id = (*it)->id.c_str();
    rapidjson::Value valId(rapidjson::StringRef(id));

    //add all that to the single reversion object
    reversionNode.AddMember(mCELL_ID,valId,allocator);
    reversionNode.AddMember(mCONTENTS,val,allocator);
    reversionNode.AddMember(mcONTENTS_TYPE,valType,allocator);

    //as well as whether the reversion object was created
    //as a byproduct of a reversion
    if((*it)->reverted)
      reversionNode.AddMember(mREVERTED,mTRUE,allocator);
    else
      reversionNode.AddMember(mREVERTED,mFALSE,allocator);

    //get the dependencies as an array and add them to the
    //single reversion node
    set<string> dependees = (*it)->dependees;
    set<string> :: iterator dependencyIt = dependees.begin();
    string dependeeList[dependees.size()];
    int ii=0;
    while(dependencyIt != dependees.end())
    {
      dependeeList[ii]=*dependencyIt;
      dependencyIt++;
      ii++;
    }
    addList(mDEPENDENCIES, dependeeList, dependees.size(), reversionNode, sheetallocator);

    //Name this single reversion with an int incase JSON puts things out
    //of order, This is not used at the moment because everything seems to be in order
    string result;
    ostringstream convert;
    convert<< rcounter;
    result = convert.str();
    const char* r = result.c_str();
    rapidjson::Value numVal;
    numVal.SetString(r,strlen(r),allocator);


    //pack the individual reversion object into the larger reversion object
    Jrevis.AddMember(numVal,reversionNode,allocator);
    it++;
    rcounter++;
  }
  //Then pack the larger reversion object into the base object
  document.AddMember(mREVERSIONS, Jrevis, allocator);

  //and fetch me string
  rapidjson::StringBuffer strbuf;
  rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
  document.Accept(writer);

  return strbuf.GetString();
}
