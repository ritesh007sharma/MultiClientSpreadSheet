#ifndef NetController_H
#define NetController_H

#include "SocketState.h"
#include <string>
#include <thread>
#include <vector>
#include "spreadsheet.h"

#define PORTNO 2112 // TODO -- Change this

// Struct for bundling connection info
struct ConnectionBundle{
        ConnectionBundle():sheet(NULL){}
        std::thread *t;
        SocketState *state;
        Spreadsheet *sheet;
};

class NetController
{
private:
        //NetController();

        std::thread *connectionThread;
        std::thread *listenThread; // Thread responsible to listening for a connection

        std::vector<ConnectionBundle> *connections;

        void (*receiver)(ConnectionBundle*, std::string); // Callback for when client data is received

        void (*clientConnected)(ConnectionBundle*); // Callback for when a client is connected

        //static NetController *instance;

        // Function ran in listening thread
        void listenForClient();

        // Function for listening for client messages.
        void listenForClientMessage(ConnectionBundle *bundle);

        void checkForClientMessages();
public:
        //static NetController* getInstance();
        NetController();
        /**
         * delete list of clients
         */
        ~NetController();

        // Returns the current list of connected clients
        std::vector<ConnectionBundle> getCurrentConnectionList();

        /**
         * Register a callback to be called when a message is received from a client.
         * The function should accept an integer containing the socket identifier
         * for the related client.
         */
        void registerReceiver(void (*f)(ConnectionBundle*, std::string));

        void registerClientReceiver(void (*f)(ConnectionBundle*));

        /**
         * start listening for clients
         */
        void beginConnect();

        // TODO - Not sure this method makes sense
        /**
         * add connected client to client list
         */
        void addClient(SocketState client);
        /**
         * remove a client from the list when they time out
         */
        void removeClient(SocketState client);
        /**
         * send message to a single client with the client ID
         */
        void sendData(std::string msg, int clientID);
        /**
         * get message from a specific client's socket state
         * if there is a complete message available.
         * the "clientID" is updated to reflect
         * the client who is making the change.
         */
        std::string receive(ConnectionBundle *bundle);

};


#endif
