/**
 * Demonstrates using rapidjson to build a json string.
 *
 * Author: Aaron Pabst
 */
#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"
#include <iostream>

using namespace std;

int main(){
        rapidjson::Document document; // Create data model object

        document.SetObject(); // Initilize object

        // Backing data object
        rapidjson::Value array(rapidjson::kArrayType);

        // Dynamic allocator for rapid json
        rapidjson::Document::AllocatorType& allocator = document.GetAllocator();

        // Build object model
        document.AddMember("type", "open", allocator);
        document.AddMember("name", "cool.sprd", allocator);
        document.AddMember("username", "pa", allocator);
        document.AddMember("password", "Doofus", allocator);

        // Convert DOM to string
        rapidjson::StringBuffer strbuf;
        rapidjson::Writer<rapidjson::StringBuffer> writer(strbuf);
        document.Accept(writer);

        cout << strbuf.GetString() << endl;
}
