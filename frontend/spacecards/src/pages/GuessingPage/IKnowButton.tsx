import { Button, Tooltip } from 'antd';
import React from 'react';
interface IKnowButtonProps {
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
  setStatistics: (cardId: Number, successValue: Number) => void;
  cardId: number;
  successValueList: string;
  setSuccessValueList: React.Dispatch<React.SetStateAction<string>>;
}

function IKnowButton(props: IKnowButtonProps) {
  const successGuessing = 1;

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
      I know
    </Button>
  );
}

export default IKnowButton;
