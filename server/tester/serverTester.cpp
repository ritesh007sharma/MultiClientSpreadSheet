
#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <string.h>
#include <arpa/inet.h>
#include <thread>

#include <iostream>

#define PORT 2112

using namespace std;

int main(){
        /*thread t1([]() {
                        string addr = "127.0.0.1";
                        int port = 2112;

                        string open = "{\"type\":\"open\",\"name\":\"cool.sprd\",\"username\":\"pajensen\",\"password\":\"Doofus\"}\n\n";

                        string edit = "{\"type\":\"edit\",\"cell\":\"B2\",\"value\":42,\"dependencies\":[]}\n\n";
                        string edit2 = "{\"type\":\"edit\",\"cell\":\"A2\",\"value\":69,\"dependencies\":[]}\n\n";


                        struct sockaddr_in address;
                        int sock = 0, valread;
                        struct sockaddr_in serv_addr;
                        //char *hello = "Hello from client";
                        char buffer[1024] = {0};
                        char listBuff[1024] = {0};
                        if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0)
                        {
                                printf("\n Socket creation error \n");
                                return -1;
                        }

                        memset(&serv_addr, '0', sizeof(serv_addr));

                        serv_addr.sin_family = AF_INET;
                        serv_addr.sin_port = htons(PORT);

                        // Convert IPv4 and IPv6 addresses from text to binary form
                        if(inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr)<=0)
                        {
                                printf("\nInvalid address/ Address not supported \n");
                                return -1;
                        }

                        if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0)
                        {
                                printf("\nConnection Failed \n");
                                return -1;
                        }
                        valread = read(sock, listBuff, 1024);
                        cout << string(listBuff) << endl;

                        send(sock, open.c_str(), strlen(open.c_str()), 0);
                        valread = read(sock, buffer, 1024);

                        //cout << edit << endl;
                        send(sock, edit.c_str(), strlen(edit.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;

                        //send(sock, edit2.c_str(), strlen(edit2.c_str()), 0);
                        //valread = read(sock, buffer, 1024);
                        //cout << string(buffer) << endl;
                        });*/

        thread t2([]() {
                        string addr = "127.0.0.1";
                        int port = 2112;

                        string open = "{\"type\":\"open\",\"name\":\"cool.sprd\",\"username\":\"pajensen\",\"password\":\"Doofus\"}\n\n";

                        string edit = "{\"type\":\"edit\",\"cell\":\"B2\",\"value\":42,\"dependencies\":[]}\n\n";
                        string edit2 = "{\"type\":\"edit\",\"cell\":\"A2\",\"value\":69,\"dependencies\":[]}\n\n{\"type\":\"edit\",\"cell\":\"A12\",\"value\":69,\"dependencies\":[]}\n\n";

                        //string edit2 = "{\"type\":\"edit\",\"cell\":\"A10\",\"value\":69,\"dependencies\":[]}\n\n";


                        struct sockaddr_in address;
                        int sock = 0, valread;
                        struct sockaddr_in serv_addr;
                        //char *hello = "Hello from client";
                        char buffer[1024] = {0};
                        char listBuff[1024] = {0};
                        if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0)
                        {
                                printf("\n Socket creation error \n");
                                return -1;
                        }

                        memset(&serv_addr, '0', sizeof(serv_addr));

                        serv_addr.sin_family = AF_INET;
                        serv_addr.sin_port = htons(PORT);

                        // Convert IPv4 and IPv6 addresses from text to binary form
                        if(inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr)<=0)
                        {
                                printf("\nInvalid address/ Address not supported \n");
                                return -1;
                        }

                        if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0)
                        {
                                printf("\nConnection Failed \n");
                                return -1;
                        }
                        valread = read(sock, listBuff, 1024);
                        cout << string(listBuff) << endl;

                        send(sock, open.c_str(), strlen(open.c_str()), 0);
                        valread = read(sock, buffer, 1024);

                        //cout << edit << endl;
                        send(sock, edit.c_str(), strlen(edit.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;

                        send(sock, edit2.c_str(), strlen(edit2.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;

                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;
        });

        /*thread t3([]() {
                        string addr = "127.0.0.1";
                        int port = 2112;

                        string open = "{\"type\":\"shutdown\"}";

                        string edit = "{\"type\":\"edit\",\"cell\":\"B2\",\"value\":4242,\"dependencies\":[]}";
                        string edit2 = "{\"type\":\"edit\",\"cell\":\"A2\",\"value\":6969,\"dependencies\":[]}";


                        struct sockaddr_in address;
                        int sock = 0, valread;
                        struct sockaddr_in serv_addr;
                        //char *hello = "Hello from client";
                        char buffer[1024] = {0};
                        char listBuff[1024] = {0};
                        if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0)
                        {
                                printf("\n Socket creation error \n");
                                return -1;
                        }

                        memset(&serv_addr, '0', sizeof(serv_addr));

                        serv_addr.sin_family = AF_INET;
                        serv_addr.sin_port = htons(PORT);

                        // Convert IPv4 and IPv6 addresses from text to binary form
                        if(inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr)<=0)
                        {
                                printf("\nInvalid address/ Address not supported \n");
                                return -1;
                        }

                        if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0)
                        {
                                printf("\nConnection Failed \n");
                                return -1;
                        }
                        valread = read(sock, listBuff, 1024);
                        cout << string(listBuff) << endl;

                        send(sock, open.c_str(), strlen(open.c_str()), 0);
                        valread = read(sock, buffer, 1024);

                        //cout << edit << endl;
                        send(sock, edit.c_str(), strlen(edit.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;

                        send(sock, edit2.c_str(), strlen(edit2.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;
                        });*/

        // Wait for threads to complete
        //t1.join();
        t2.join();
        //t3.join();

        /*thread t4([]() {
                        string addr = "127.0.0.1";
                        int port = 2112;

                        string open = "{\"type\":\"shutdown\"}";

                        //string edit = "{\"type\":\"edit\",\"cell\":\"B2\",\"value\":4242,\"dependencies\":[]}";
                        //string edit2 = "{\"type\":\"edit\",\"cell\":\"A2\",\"value\":6969,\"dependencies\":[]}";


                        struct sockaddr_in address;
                        int sock = 0, valread;
                        struct sockaddr_in serv_addr;
                        //char *hello = "Hello from client";
                        char buffer[1024] = {0};
                        char listBuff[1024] = {0};
                        if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0)
                        {
                                printf("\n Socket creation error \n");
                                return -1;
                        }

                        memset(&serv_addr, '0', sizeof(serv_addr));

                        serv_addr.sin_family = AF_INET;
                        serv_addr.sin_port = htons(PORT);

                        // Convert IPv4 and IPv6 addresses from text to binary form
                        if(inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr)<=0)
                        {
                                printf("\nInvalid address/ Address not supported \n");
                                return -1;
                        }

                        if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0)
                        {
                                printf("\nConnection Failed \n");
                                return -1;
                        }
                        valread = read(sock, listBuff, 1024);
                        cout << string(listBuff) << endl;

                        send(sock, open.c_str(), strlen(open.c_str()), 0);
                        //valread = read(sock, buffer, 1024);

                        //cout << edit << endl;
                        send(sock, edit.c_str(), strlen(edit.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;

                        send(sock, edit2.c_str(), strlen(edit2.c_str()), 0);
                        valread = read(sock, buffer, 1024);
                        cout << string(buffer) << endl;
        });

        t4.join();*/
}
