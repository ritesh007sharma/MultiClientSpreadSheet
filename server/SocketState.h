#ifndef SocketState_H
#define SocketState_H

#include <string>

class SocketState
{
private:
        int id;
public:
        /**
         * create a socket state for
         */
        SocketState(int ID);
        /**
         * return a single message when called
         * if there is no messages available
         * return '\0'
         */ 
        std::string getMessage();

        int getId();
};



#endif
