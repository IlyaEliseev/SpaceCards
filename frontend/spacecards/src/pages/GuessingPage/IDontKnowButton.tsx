import { SearchOutlined, CloseOutlined } from '@ant-design/icons';
import { Button, Tooltip } from 'antd';
import React from 'react';
interface IDontKnowButtonProps {
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
  setStatistics: (cardId: Number, successValue: Number) => void;
  cardId: Number;
  successValueList: string;
  setSuccessValueList: React.Dispatch<React.SetStateAction<string>>;
}

function IDontKnowButton(props: IDontKnowButtonProps) {
  const successGuessing = 0;

  return (
    <Button
      size='large'
      onClick={() => {
        props.setCount(props.count + 1);
        props.setSuccessValueList(
          props.successValueList + String(successGuessing)
        );
        props.setStatistics(props.cardId, successGuessing);
      }}
    >
      I don't know
    </Button>
  );
}

export default IDontKnowButton;
