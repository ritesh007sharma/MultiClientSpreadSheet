///
/// Demonstrates using rapidjson to decode a json string and pull
//  out individual fields
///
/// Author: Aaron Pabst
///
///

#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"
#include <iostream>

using namespace std;

int main(){

        // JSon string to decode (open command)
        char* jsonString = "{\"type\":\"open\", \"name\":\"cool.sprd\",\"username\":\"pa\",\"password\":\"Doofus\"}";

        // parse the json into a dom
        rapidjson::Document d;
        d.Parse(jsonString);

        cout << d["name"].GetString() << endl;
}
