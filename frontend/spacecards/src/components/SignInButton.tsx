import React, { useState } from 'react';
import { Button } from 'antd';
import type { SizeType } from 'antd/es/config-provider/SizeContext';
import { GoogleOutlined, ImportOutlined } from '@ant-design/icons';

function GoogleAuthButton() {
  const [size, setSize] = useState<SizeType>('middle');
  const [showSignInForm, setShowSignInForm] = useState(false);

  const showForm = () => {
    setShowSignInForm((showSignInForm) => !showSignInForm);
    console.log(showSignInForm);
  };

  return (
    <Button
      icon={<ImportOutlined rotate={180} />}
      size={size}
      onClick={() => showForm()}
    >
      Sign in
    </Button>
  );
}

export default GoogleAuthButton;
