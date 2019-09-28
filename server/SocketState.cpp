
#include "SocketState.h"

using namespace std;

SocketState::SocketState(int ID){
        id = ID;
}

string SocketState::getMessage(){
        return "";
}

int SocketState::getId(){
        return id;
}
