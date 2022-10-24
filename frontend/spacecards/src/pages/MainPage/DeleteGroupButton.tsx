import React, { useState, useEffect } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
interface DeleteGroupButtonProps {
  id: number;
  deleteGroup: (value: number) => void;
}
function DeleteGroupButton(props: DeleteGroupButtonProps) {
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
