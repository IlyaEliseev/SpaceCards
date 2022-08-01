import { Card } from 'antd';
import React from 'react';

function GroupCard(props: {
  key: number;
  id: number;
  frontSide: string;
  backSide: string;
}) {
  return (
    <Card
      key={props.id}
      title={props.frontSide}
      size='default'
      bordered={false}
      style={{ width: 250, height: 185 }}
      hoverable={true}
      actions={[]}
    ></Card>
  );
}

export default GroupCard;
