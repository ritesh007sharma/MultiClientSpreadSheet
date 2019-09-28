#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <sstream>
#include <string>
#include <vector>
#include <map>
#include <stack>
#include <ostream>
#include <iostream>

#include "dependencyGraph.h"

/**
 * @brief Keep track of the contents of a cell, as well as what type
 * it should be represented as
 */
struct ContentsVar{
  std::string _contents;
  char type;
  ContentsVar():_contents("")
  {
    type='s';
  }
  ContentsVar(std::string contents):_contents(contents)
  {
    type='s';
  }
  ContentsVar(double contents)
  {
    if(static_cast<long>(contents) < contents){
      type='d';
    std::string result;
    std::ostringstream convert;
    convert<< contents;
    _contents = convert.str();
    }
    else{
      type='i';
        std::string result;
    std::ostringstream convert;
    convert<< static_cast<long>(contents);
    _contents = convert.str();
    }


  
  }
};


struct Reversion {
    Reversion(std::string i, ContentsVar p, std::set<std::string> d)
        : id(i), contents(p), dependees(d), reverted(false) {};
    std::string id;
    ContentsVar contents;
    std::set<std::string> dependees;

    //Added this variable so that we know if an
    //element of the undo stack has been a revision
    //so taht it can skip it (otherwise, it jsut finds it, and
    //"reverts" back and forth between the two most recent states.
    // -Daniel
    bool reverted;
};



class Spreadsheet
{

private:
        std::string spreadSheetName;
        std::string spreadSheetLoc;
    public:
        /**
         * create a filled spreadsheet
         */
        //Spreadsheet();

        Spreadsheet(std::string name);

        ~Spreadsheet();
        std::string messageHandler(std::string);
        std::string getSpreadsheet();
        void loadSpreadsheet(std::string);
        
  //  private:
        /**
         * add a cell with the supplied info
         */
        std::string edit(std::string cell, std::string contents, std::set<std::string> dependencies);
        std::string edit(std::string cell, double contents, std::set<std::string> dependencies);
        /**
         * undo spreadsheet
         */
        std::string undo();
        /**
         * revert cell
         */
        std::string revert(std::string cell);

        /**
         * get all cells contents as a vector
         */
        std::string getFullSend();
        std::string getDelta(std::string, ContentsVar);
        std::string getCircularError(std::string);
        std::vector<Reversion*> history;
        DependencyGraph dependencies;
        std::map<std::string, ContentsVar> cells;
};

#endif
