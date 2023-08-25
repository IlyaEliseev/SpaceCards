import React, { useState } from 'react';

export const useLoginForm = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  const handleEmail = (emailProp: string) => {
    setEmail(emailProp);
  };

  const handlePassword = (passwordProp: string) => {
    setPassword(passwordProp);
  };

  return { email, password, handleEmail, handlePassword };
};
