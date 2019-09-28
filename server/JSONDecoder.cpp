
#include "JSONDecoder.h"
#include "rapidjson/document.h"
#include "rapidjson/stringbuffer.h"

using namespace std;

rapidjson::Document JSONDecoder::decode(string msg){
        rapidjson::Document d;
        d.Parse(msg.c_str());

        return rapidjson::Document(d);
}
