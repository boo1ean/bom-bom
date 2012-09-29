#ifndef GAME_H
#define GAME_H

#include <QObject>
#include <server.h>
#include <connection.h>
#include <clientscreenobject.h>
#include <clienttype.h>
#include <player.h>
#include <observer.h>

class Game : public QObject
{
    Q_OBJECT

    Server* _server;
    QList <ClientScreenObject*> clients;

public:
    explicit Game(QObject *parent = 0);
    
signals:
    
private slots:
    void receiveConenction(Connection* connection);
    void initClient(Command command, QByteArray data);
};

#endif // GAME_H
