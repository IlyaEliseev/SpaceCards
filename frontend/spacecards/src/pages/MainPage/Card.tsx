import React, { useState } from 'react';
import { Card } from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { EditOutlined, CloseCircleOutlined } from '@ant-design/icons';
import Input from 'antd/lib/input/Input';
import GroupSelector from './GroupSelector';
import { CardType } from './Content';
interface CardComponentProps {
  key: number;
  id: number;
  frontSide: string;
  backSide: string;
  groups: never[];
  deleteCard: (value: number) => void;
  updateCard: (card: CardType) => void;
}

function CardComponent(props: CardComponentProps) {
  const [size, setSize] = useState<SizeType>('large');
  const [isClick, setClick] = useState<boolean>(false);
  const [frontSide, setFrontSide] = useState<string>('');
  const [backSide, setBackSide] = useState<string>('');
  const card = { cardId: props.id, frontSide: frontSide, backSide: backSide };

  const handleUpdateCard = () => {
    setClick(false);
    props.updateCard(card);
    clearInput();
  };

  const clearInput = () => {
    setFrontSide('');
    setBackSide('');
  };

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
            handleUpdateCard();
          }}
        />,
      ]}
    >
      <Input
        placeholder='Word'
        value={frontSide}
        onChange={(e) => {
          setFrontSide(e.target.value);
        }}
      />
      <Input
        placeholder='Translate'
        value={backSide}
        onChange={(e) => {
          setBackSide(e.target.value);
        }}
      />
    </Card>
  );
}

export default CardComponent;
