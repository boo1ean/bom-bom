#include "connection.h"
#include <command.h>

Connection::Connection(QTcpSocket* socket, QObject *parent) :
    QObject(parent)
{
    connect(socket, SIGNAL(disconnected()),
            socket, SLOT(deleteLater()));

    // Execute moveRect method if there is new data in socket
    connect(socket, SIGNAL(readyRead()),
            this,   SLOT(onReadyRead()));

    this->socket = socket;
}

void Connection::onReadyRead() {

    QByteArray data = socket->readAll();
    char command = data.at(0);

    // Check if command is available
    switch(command) {
        case CMD_INIT:
        case CMD_NAME:
        case CMD_ACC_DATA:
        case CMD_NOTIFY:
        case CMD_SCENE_STATE:
            // Available command
        break;

        default:
            throw new std::exception();
    }

    emit newCommand((Command)command, data.right(data.size() - 1));
}
