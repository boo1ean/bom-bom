#ifndef CLIENTSCREENOBJECT_H
#define CLIENTSCREENOBJECT_H
#include <screenobject.h>
#include <connection.h>

class ClientScreenObject : public ScreenObject
{
public:
    ClientScreenObject(Connection* connection, QObject *parent = 0);
};

#endif // CLIENTSCREENOBJECT_H
