import React, { useState } from 'react';
import { Card, Button } from 'antd';
import 'antd/dist/antd.min.css';
import {
  EditOutlined,
  EllipsisOutlined,
  SettingOutlined,
} from '@ant-design/icons';
import type { SizeType } from 'antd/es/config-provider/SizeContext';

function CardComponent(props: {
  id: number;
  frontSide: string;
  backSide: string;
}) {
  const [size, setSize] = useState<SizeType>('large');

  return (
    <div className='site-card-border-less-wrapper'>
      <Card
        key={props.id}
        title='Card title'
        size='default'
        extra={
          <Button type='primary' size={size}>
            Primary
          </Button>
        }
        bordered={false}
        style={{ width: 300 }}
        hoverable={true}
      >
        <p>{props.frontSide}</p>
        <p>{props.backSide}</p>
      </Card>
    </div>
  );
}

export default CardComponent;
