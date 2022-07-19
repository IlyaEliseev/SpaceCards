import { SearchOutlined, CloseOutlined } from '@ant-design/icons';
import { Button, Tooltip } from 'antd';
import React from 'react';

function DeleteCardButton(props: {
  id: number;
  deleteCard: (value: number) => void;
}) {
  return (
    <Button
      type='primary'
      shape='circle'
      size='small'
      onClick={() => {
        props.deleteCard(props.id);
      }}
    >
      <CloseOutlined />
    </Button>
  );
}

export default DeleteCardButton;
