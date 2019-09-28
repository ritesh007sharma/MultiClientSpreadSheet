#ifndef ENCODER_H
#define ENCODER_H



#define mDEPENDENCIES "D"
#define mCELLS "cells"
#define mATTRIBUTES "attributes"
#define mSPREADSHEET_NAME "name"
#define mSAVEFILE_VERSION "version"
#define mCONTENTS "="
#define mCELL_ID "I"
#define mREVERTED "R"
#define mcONTENTS_TYPE "T"
#define mREVERSIONS "Reversions"
#define mTRUE "1"
#define mFALSE "0"


//#include "Cell.h"
#include "dependencyGraph.h"
#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"
#include "spreadsheet.h"
#include <vector>

typedef struct{
        std::string location;
        std::string contents;
        char type;
}cell;

/**
 * Provides some static methods and structures for packing information into
 * protocol standard message formats
 *
 * Author: Aaron Pabst
 */
class JSONEncoder{
public:

        // Pack an error message into a JSON string
        static std::string packError(int code, std::string source);

        // Pack a list of spreadsheets into a list
        static std::string packList(std::string names[], int length);

        // Pack a sheet
        static std::string packSheet(cell cells[]);

        static void addList(std::string category, std::string names[], int len, rapidjson::Document& toDocument, rapidjson::Document::AllocatorType& alloc);
        static std::string packSheet(cell cells[], int length, std::string spdName, DependencyGraph dGraph, std::vector<Reversion*>);
        static std::string packUsers(std::map<std::string,std::string>);

};

#endif
