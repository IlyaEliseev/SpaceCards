import React, { useState, useEffect } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
interface AddGroupButtonProps {
  createGroup: () => void;
  isVisibleCreateInput: boolean;
  setIsVisibleCreateInput: React.Dispatch<React.SetStateAction<boolean>>;
}

function AddGroupButton(props: AddGroupButtonProps) {
  const [size, setSize] = useState<SizeType>('middle');

  return (
    <Button
      type='primary'
      size={size}
      onClick={() => {
        props.createGroup();
        props.setIsVisibleCreateInput(!props.isVisibleCreateInput);
      }}
    >
      Add group
    </Button>
  );
}

export default AddGroupButton;
