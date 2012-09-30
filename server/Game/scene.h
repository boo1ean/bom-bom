#ifndef SCENE_H
#define SCENE_H

#include <QObject>
#include <Box2D.h>
#include <QGraphicsScene>
#include <QTimer>
#include "wall.h"

class Scene : public QObject
{
    Q_OBJECT

    b2World *_world;
    QGraphicsScene *_scene;

    b2Body *_ball;

    QList<Wall*> _walls;

    QTimer *_time;

    void initGraphicsScene();

public:
    Scene(QObject *parent = 0);

    b2World* getPhysics() const;
    QGraphicsScene* getGraphics() const;
    
signals:
    
private slots:

    void onNewFrame();
    
};

#endif // SCENE_H
