#ifndef SCENE_H
#define SCENE_H

#include <QObject>
#include <Box2D.h>

class Scene : public QObject
{
    Q_OBJECT

    b2World* _world;

public:
    explicit Scene(QObject *parent = 0);
    
signals:
    
public slots:
    
};

#endif // SCENE_H
