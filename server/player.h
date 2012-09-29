#ifndef PLAYER_H
#define PLAYER_H
#include <clientscreenobject.h>
#include <connection.h>

class Player : public ClientScreenObject
{
public:
    Player(Connection* connection, QObject *parent = 0);
};

#endif // PLAYER_H
