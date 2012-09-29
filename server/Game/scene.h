#ifndef SCENE_H
#define SCENE_H

#include <QObject>
#include <Box2D.h>
#include <QGraphicsScene>

class Scene : public QObject
{
    Q_OBJECT

    b2World *_world;
    QGraphicsScene *_scene;

public:
    explicit Scene(QObject *parent = 0);
    
signals:
    
public slots:
    
};

#endif // SCENE_H
