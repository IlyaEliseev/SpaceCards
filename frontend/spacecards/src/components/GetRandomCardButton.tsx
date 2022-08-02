import { Button, Tooltip } from 'antd';
import React from 'react';

function GetRandomCardButton() {
  return (
    <Button type='primary' size='middle' href='/guessingCards'>
      Start guessing card
    </Button>
  );
}

export default GetRandomCardButton;
