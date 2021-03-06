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

function CardCreation(props: {
  setFrontSide: React.Dispatch<React.SetStateAction<string>>;
  setBackSide: React.Dispatch<React.SetStateAction<string>>;
  createCard: () => void;
}) {
  const [size, setSize] = useState<SizeType>('large');
  const [isClick, setClick] = useState(false);
  const [frontSide, setfrontSide] = useState('');
  const [backSide, setbackSide] = useState('');

  const clearinput = () => {
    setfrontSide('');
    setbackSide('');
  };

  const content = (
    <div>
      <Input placeholder='Translate' maxLength={2} size={'small'} />
      <Input placeholder='Translate' maxLength={100} />
    </div>
  );

  return (
    <Card
      size='default'
      bordered={false}
      style={{ width: 250 }}
      hoverable={true}
      actions={[
        <EditOutlined
          key='edit'
          onClick={() => {
            setClick(false);
            props.createCard();
            props.setFrontSide('');
            props.setBackSide('');
            clearinput();
          }}
        />,
      ]}
    >
      <Input
        placeholder='Word'
        value={frontSide}
        onChange={(e) => {
          props.setFrontSide(e.target.value);
          setfrontSide(e.target.value);
        }}
      />
      <Input
        placeholder='Translate'
        value={backSide}
        onChange={(e) => {
          props.setBackSide(e.target.value);
          setbackSide(e.target.value);
        }}
      />
    </Card>
  );
}

export default CardCreation;
