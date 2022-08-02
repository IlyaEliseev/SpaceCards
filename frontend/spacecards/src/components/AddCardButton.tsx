import { Button, Tooltip } from 'antd';
import React from 'react';

function AddCardButton(props: { createCard: () => void }) {
  return (
    <Button
      type='primary'
      shape='circle'
      size='large'
      onClick={() => {
        props.createCard();
      }}
    >
      +
    </Button>
  );
}

export default AddCardButton;
