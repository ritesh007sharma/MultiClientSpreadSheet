/**
 * Provides an abstracted web interface.
 *
 * Author: Aaron Pabst
 */

#include "NetController.h"
#include "SocketState.h"

#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <thread>
#include <vector>
#include <iostream>
#include <string.h>
#include <signal.h>

using namespace std;

//NetController* NetController::instance = 0;

NetController::NetController(){
        cout << "NetController initialized..." << endl;
        connections = new vector<ConnectionBundle>();
        signal(SIGPIPE, SIG_IGN); // Ignore broken pipe exceptions
}

/*NetController* NetController::getInstance(){
        if(instance == 0)
             instance = new NetController();
        return instance;
        }*/

NetController::~NetController(){
        //listenThread->terminate();
        delete connections;
        delete listenThread;
        delete connectionThread;
}

vector<ConnectionBundle> NetController::getCurrentConnectionList(){
        return vector<ConnectionBundle>(*connections); // Return a copy
}

void NetController::registerReceiver(void (*f)(ConnectionBundle*, string)){
        receiver = f;
}

void NetController::registerClientReceiver(void (*f)(ConnectionBundle*)){
        clientConnected = f;
}

void NetController::listenForClientMessage(ConnectionBundle *bundle){
        cout << "Listening for client message" << endl;

        while(1){
                cout << "listening...";
                char buff[1024] = {0};
                int len = read(bundle->state->getId(), buff, 1024);

                if(len <= 0){ // message complete
                        cout << "Message complete..." << endl;
                        break;
                }

                cout << "got data..." << endl;

                // Split data up around newlines, in case the packet
                //  has multiple messages in the buffer
                string delim = "\n\n";
                string data = string(buff);
                //string token;
                size_t pos = 0;

                vector<string> cmds;
                char* token = strtok(const_cast<char*>(data.c_str()), delim.c_str());
                while(token != nullptr){
                        cmds.push_back(string(token));
                        token = strtok(nullptr, delim.c_str());
                }

                // while((pos = data.find(delim)) != string::npos){
                //         token = data.substr(0, pos);
                //         receiver(bundle, token); // Pass data downstream
                //         data.erase(0, pos + delim.length());
                // }
                for(auto &cmd : cmds){
                        receiver(bundle, cmd);
                }
        }
}

void NetController::checkForClientMessages(){
        while(1){
                for(int i = 0; i < connections->size(); i++){
                        int sockfd = connections->data()[i].state->getId();

                        char buff[1024] = {0};

                        fd_set sready;
                        struct timeval to;
                        to.tv_sec = 0;
                        to.tv_usec = 100;

                        FD_ZERO(&sready);
                        FD_SET(sockfd, &sready);

                        int sockstate = select(sockfd+1, &sready, NULL, NULL, &to);
                        //cout << sockstate << endl;

                        // Only read if data is ready
                        if(sockstate){
                                int len = read(sockfd, buff, 1024);


                                if(len >  0){ // data to be read
                                        cout << "Data receieved: " << string(buff) << endl;

                                        string data = string(buff);
                                        string delim = "\n\n";

                                        vector<string> cmds;
                                        char* token = strtok(const_cast<char*>(data.c_str()), delim.c_str());
                                        while(token != nullptr){
                                                cmds.push_back(string(token));
                                                token = strtok(nullptr, delim.c_str());
                                        }

                                        for(auto &cmd : cmds){
                                                receiver(&connections->data()[i], cmd);
                                        }
                                }
                                else
                                {
                                        
                                        receiver(&connections->data()[i], "{\"type\":\"disconnect\"}");
                                        
                                        connections->erase(connections->begin()+i);
                                      
                                        close(sockfd);
                                }
                                
                        }
                }
        }
}

void NetController::listenForClient(){
        int opt = 1;
        int fd = socket(AF_INET, SOCK_STREAM, 0);
        if(fd == 0){
                cout << "Couldn't create socket..." << endl;
        }

        // Setup socket options
        if(setsockopt(fd, SOL_SOCKET, SO_REUSEADDR | SO_REUSEPORT, &opt, sizeof(opt))){
                cout << "Unable to set socket options" << endl;
                return;
        }

        // Initilize address information
        struct sockaddr_in addr;
        addr.sin_family = AF_INET;
        addr.sin_addr.s_addr = INADDR_ANY;
        addr.sin_port = htons(PORTNO);

        int addrlen = sizeof(addr);

        if(bind(fd, (struct sockaddr*)&addr, sizeof(addr)) < 0){ // Bind the socket to the address, port
                cout << "Could not bind socket..." << endl;
                return;
        }

        if(listen(fd, 3) < 0){ // Second parameter is how many concurrent connections can be queued
                cout << "Could not begin listening for client connections..." << endl;
                return;
        }
        cout << "Listening for clients on " << addr.sin_addr.s_addr << ", " << addr.sin_port << endl;

        while(1){
                //checkForClientMessages();
                int newSock = accept(fd, (struct sockaddr *)&addr, (socklen_t*)&addrlen);
                if(newSock < 0){
                     cout << "Error accepting connection..." << endl;
                return;
        }

        // New client connected at this point
        cout << "New client connected" << endl;

        // Throw info into the connection vector
        ConnectionBundle bundle;
        bundle.state = new SocketState(newSock);
        //bundle.t = new thread(listenForClientMessage, &bundle); // Begin listening for messages

        connections->push_back(bundle); // Add the connection information to the list

        cout << "finished adding connection to list..." << endl;

        clientConnected(&bundle); // Notify of new connection
        }
        //NetController::getInstance()->beginConnect(); // Start connection cycle again
}

void NetController::beginConnect(){
        if(listenThread != 0)
                delete listenThread; // Ensure the old thread is stopped

        connectionThread = new thread(&NetController::listenForClient, this); // Fire up new thread
        listenThread = new thread(&NetController::checkForClientMessages, this);
}

void NetController::addClient(SocketState client){

}

void NetController::removeClient(SocketState client){

}

void NetController::sendData(string msg, int clientID){
        //if(is_client_closed(clientID))
                send(clientID, msg.c_str(),msg.length(), 0);
}

// Currently, this function kicks up a new thread, and calls the registers
// callback function when data is available
// TODO - Remove this function
string NetController::receive(ConnectionBundle *bundle){
        //thread *th = new thread(&NetController::listenForClientMessage, this);
        //cout << "Thread started..." << endl;
        //bundle->t = th;
        //cout << "Receive done..." << endl;
}
