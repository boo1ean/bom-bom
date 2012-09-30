#include "sceneitem.h"
#include <QGraphicsItem>
#include <QDebug>

SceneItem::SceneItem(QObject *parent) :
    QObject(parent)
{
}

void SceneItem::update()
{
    QGraphicsItem *item = (QGraphicsItem *)_body->GetUserData();
    const b2Vec2& pos = _body->GetPosition();
    item->setPos(pos.x, pos.y);
}
