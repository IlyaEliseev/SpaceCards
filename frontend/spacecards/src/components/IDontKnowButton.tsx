import { SearchOutlined, CloseOutlined } from '@ant-design/icons';
import { Button, Tooltip } from 'antd';
import React from 'react';

function IDontKnowButton(props: {
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
  setStatistics: (cardId: Number, successValue: Number) => void;
  cardId: Number;
}) {
  const successGuessing = 0;

  return (
    <Button
      size='large'
      onClick={() => {
        props.setCount(props.count + 1);
        props.setStatistics(props.cardId, successGuessing);
      }}
    >
      I dont know
    </Button>
  );
}

export default IDontKnowButton;
