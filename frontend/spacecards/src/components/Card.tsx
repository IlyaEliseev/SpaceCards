import React, { useState } from 'react';
import { Card, Button, Popover } from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { MenuClickEventHandler } from 'rc-menu/lib/interface';
import {
  EditOutlined,
  EllipsisOutlined,
  SettingOutlined,
} from '@ant-design/icons';
import { Avatar, Skeleton, Switch } from 'antd';
import GithubAuthButton from './GithubAuthButton';
import DeleteCardButton from './DeleteCardButton';
import Input from 'antd/lib/input/Input';

function CardComponent(props: {
  key: number;
  id: number;
  frontSide: string;
  backSide: string;
  deleteCard: (value: number) => void;
}) {
  const [size, setSize] = useState<SizeType>('large');
  const [frontSide, setFrontSide] = useState('');
  console.log(frontSide);

  const content = (
    <div>
      <Input placeholder='Translate' maxLength={2} size={'small'} />
      <Input placeholder='Translate' maxLength={100} />
    </div>
  );

  return (
    <Card
      key={props.id}
      title={props.frontSide}
      size='default'
      bordered={false}
      style={{ width: 250 }}
      hoverable={true}
      actions={[
        <EditOutlined key='edit' onPointerEnter={() => console.log('1')} />,
        <DeleteCardButton id={props.id} deleteCard={props.deleteCard} />,
      ]}
    >
      <Input
        placeholder='Word'
        onChange={(e) => setFrontSide(e.target.value)}
      />
      <Popover content={content} title='Title'>
        <Button type='primary'>Hover me</Button>
      </Popover>
      <Input placeholder='Translate' />
    </Card>
  );
}

export default CardComponent;
