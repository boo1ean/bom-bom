#ifndef GAME_H
#define GAME_H

#include <QApplication>
#include "server.h"
#include "connection.h"
#include "clientscreenobject.h"
#include "scene.h"

class Game : QObject
{
    Q_OBJECT

    Server* _server;
    Scene* _scene;

    QList <ClientScreenObject*> clients;

public:
    explicit Game(/*int argc, char* argv[]*/);
    
signals:
    
private slots:
    void addClient(Connection* connection);
    void initClient(Command command, QByteArray data);
};

#endif // GAME_H
