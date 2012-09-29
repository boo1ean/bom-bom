#include "game.h"
#include <exception>

Game::Game(QObject *parent) :
    QObject(parent)
{
    _server = new Server;

    connect(_server, SIGNAL(newConnection(Connection*)),
            this,    SLOT(receiveConnection(Connection*)));
}

void Game::receiveConenction(Connection *connection) {
    connect(connection, SIGNAL(newCommand(Command, QByteArray)),
            this,       SLOT(initClient(Command, QByteArray)));
}

void Game::initClient(Command command, QByteArray data) {
    Connection *connection = (Connection*)sender();
    ClientScreenObject * clientScreenObject;

    if (CMD_INIT == command) {
        unsigned char clientType = data[0];
        
        switch (clientType) {
            case CLIENT_BADA:
            case CLIENT_ANDROID:
            case CLIENT_WP7:
            case CLIENT_IPHONE:
            case CLIENT_SYMBIAN:
                clientScreenObject = new Player(connection);
            break;

            case CLIENT_OBSERVER:
                clientScreenObject = new Observer(connection);
            break;

            default:
                throw new std::exception();
        }

        clients.push_back(clientScreenObject);

        /* deattach event handler */
        connection->disconnect(this);
    }
}
