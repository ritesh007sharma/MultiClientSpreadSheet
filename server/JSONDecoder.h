#ifndef DECODER_H
#define DECODER_H

#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"

// Provides a mechanism for decoding a JSON message into dictonary
class JSONDecoder{
        static rapidjson::Document decode(std::string msg);
};

#endif
