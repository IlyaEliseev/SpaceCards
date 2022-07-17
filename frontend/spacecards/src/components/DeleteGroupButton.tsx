import React, { useState, useEffect } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GoogleOutlined } from '@ant-design/icons';

function DeleteGroupButton(props: {
  id: number;
  deleteGroup: (value: number) => void;
}) {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      type='primary'
      size={size}
      onClick={() => {
        props.deleteGroup(props.id);
      }}
    >
      Delete group
    </Button>
  );
}

export default DeleteGroupButton;
