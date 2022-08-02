import { SearchOutlined, CloseOutlined } from '@ant-design/icons';
import { Button, Tooltip } from 'antd';
import React from 'react';

function IKnowButton(props: {
  count: number;
  setCount: React.Dispatch<React.SetStateAction<number>>;
}) {
  const successGuessing = 1;

  return (
    <Button
      size='large'
      onClick={() => {
        props.setCount(props.count + 1);
      }}
    >
      I know
    </Button>
  );
}

export default IKnowButton;
