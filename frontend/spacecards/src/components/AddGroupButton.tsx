import React, { useState, useEffect } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GoogleOutlined } from '@ant-design/icons';

function AddGroupButton(props: { createGroup: () => void }) {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      type='primary'
      size={size}
      onClick={() => {
        props.createGroup();
      }}
    >
      Add group
    </Button>
  );
}

export default AddGroupButton;
