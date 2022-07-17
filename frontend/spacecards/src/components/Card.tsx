import React, { useState } from 'react';
import { Card, Button } from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';

function CardComponent(props: {
  id: number;
  frontSide: string;
  backSide: string;
}) {
  const [size, setSize] = useState<SizeType>('large');

  const cards = props;

  return (
    <Card
      key={props.id}
      title={props.frontSide}
      size='default'
      // extra={
      //   <Button type='primary' size={size}>
      //     Primary
      //   </Button>
      // }
      bordered={false}
      style={{ width: 250, height: 200 }}
      hoverable={true}
    >
      {/* <p>{props.backSide}</p> */}
    </Card>
  );
}

export default CardComponent;
