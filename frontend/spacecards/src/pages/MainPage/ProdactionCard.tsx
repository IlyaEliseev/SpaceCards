import React, { useState } from 'react';
import { Card } from 'antd';
import 'antd/dist/antd.min.css';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { EditOutlined } from '@ant-design/icons';
import Input from 'antd/lib/input/Input';
import { CardType } from './Content';

interface ProdactionCardProps {
  createCard: (card: CardType) => void;
}

function ProdactionCard(props: ProdactionCardProps) {
  const [size, setSize] = useState<SizeType>('large');
  const [frontSide, setfrontSide] = useState<string>('');
  const [backSide, setbackSide] = useState<string>('');

  const clearinput = () => {
    setfrontSide('');
    setbackSide('');
  };

  const handleCreateCard = () => {
    props.createCard({ cardId: 0, frontSide, backSide });
    clearinput();
  };

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
            handleCreateCard();
          }}
        />,
      ]}
    >
      <Input
        placeholder='Word'
        value={frontSide}
        onChange={(e) => {
          setfrontSide(e.target.value);
        }}
      />
      <Input
        placeholder='Translate'
        value={backSide}
        onChange={(e) => {
          setbackSide(e.target.value);
        }}
      />
    </Card>
  );
}

export default ProdactionCard;
