import React, { useState } from 'react';
import { Card, Button, Popover } from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { MenuClickEventHandler } from 'rc-menu/lib/interface';
import {
  EditOutlined,
  CloseCircleOutlined,
  EllipsisOutlined,
  SettingOutlined,
} from '@ant-design/icons';
import { Avatar, Skeleton, Switch } from 'antd';
import GithubAuthButton from './GithubAuthButton';
import DeleteCardButton from './DeleteCardButton';
import Input from 'antd/lib/input/Input';
import GroupSelector from './GroupSelector';
import Meta from 'antd/lib/card/Meta';

function CardComponent(props: {
  key: number;
  id: number;
  frontSide: string;
  backSide: string;
  groups: never[];
  deleteCard: (value: number) => void;
  setFrontSide: React.Dispatch<React.SetStateAction<string>>;
  setBackSide: React.Dispatch<React.SetStateAction<string>>;
  updateCard: (cardId: number) => void;
}) {
  const [size, setSize] = useState<SizeType>('large');
  const [isClick, setClick] = useState(false);
  const [frontSide, setFrontSide] = useState('');
  const [backSide, setBackSide] = useState('');

  const clearInput = () => {
    setFrontSide('');
    setBackSide('');
  };

  const content = (
    <div>
      <Input placeholder='Translate' maxLength={2} size={'small'} />
      <Input placeholder='Translate' maxLength={100} />
    </div>
  );

  return isClick === false ? (
    <Card
      key={props.id}
      title={props.frontSide}
      size='default'
      bordered={false}
      style={{ width: 250 }}
      hoverable={true}
      actions={[
        <EditOutlined key='edit' onClick={() => setClick(true)} />,
        <CloseCircleOutlined
          onClick={() => {
            props.deleteCard(props.id);
            clearInput();
          }}
        />,
      ]}
    >
      <GroupSelector cardId={props.id} groups={props.groups} />
    </Card>
  ) : (
    <Card
      key={props.id}
      size='default'
      bordered={false}
      style={{ width: 250 }}
      hoverable={true}
      actions={[
        <EditOutlined
          key='edit'
          onClick={() => {
            setClick(false);
            props.setFrontSide('');
            props.setBackSide('');
            props.updateCard(props.id);
          }}
        />,
      ]}
    >
      <Input
        placeholder='Word'
        value={frontSide}
        onChange={(e) => {
          props.setFrontSide(e.target.value);
          setFrontSide(e.target.value);
        }}
      />
      <Input
        placeholder='Translate'
        value={backSide}
        onChange={(e) => {
          props.setBackSide(e.target.value);
          setBackSide(e.target.value);
        }}
      />
    </Card>
  );
}

export default CardComponent;
