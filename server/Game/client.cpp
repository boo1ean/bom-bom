#include "client.h"

Client::Client(Connection *connection, QObject *parent) :
    QObject(parent), connection(connection)
{
    connect(connection, SIGNAL(newCommand(Command, QByteArray)),
            this,       SLOT(executeCommand(Command, QByteArray)));
}

void Client::executeCommand(Command command, QByteArray data) {

    switch(command) {
        case CMD_INIT:

        break;

        case CMD_NAME:

        break;

        case CMD_ACC_DATA:

        break;

        case CMD_NOTIFY:

        break;

        case CMD_SCENE_STATE:

        break;

        default:
            throw new std::exception();
    }

}
