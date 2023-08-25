import { Button, Form, Input, Select } from 'antd';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import React, { useState } from 'react';
import { RegistrationData } from './AuthPage';
interface RegistrationFormProps {
  registraion: (data: RegistrationData) => void;
}

const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/;

function RegistrationForm(props: RegistrationFormProps) {
  const [autoCompleteResult, setAutoCompleteResult] = useState<string[]>([]);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [nickname, setNickname] = useState('');

  const { Option } = Select;

  const [form] = Form.useForm();

  const onWebsiteChange = (value: string) => {
    if (!value) {
      setAutoCompleteResult([]);
    } else {
      setAutoCompleteResult(
        ['.com', '.org', '.net'].map((domain) => `${value}${domain}`)
      );
    }
  };

  const websiteOptions = autoCompleteResult.map((website) => ({
    label: website,
    value: website,
  }));

  const onFinish = async () => {
    await props.registraion({
      email: email,
      nickname: nickname,
      password: password,
    });
  };

  return (
    <div className='registration-form'>
      <Form
        form={form}
        name='register'
        onFinish={onFinish}
        initialValues={{
          residence: ['zhejiang', 'hangzhou', 'xihu'],
          prefix: '86',
        }}
        scrollToFirstError
      >
        <Form.Item
          name='nicname'
          rules={[
            {
              required: true,
              message: 'Please input your nickname',
            },
          ]}
        >
          <Input
            value={nickname}
            prefix={<UserOutlined className='site-form-item-icon' />}
            placeholder='Nickname'
            onChange={(e) => setNickname(e.target.value)}
          />
        </Form.Item>

        <Form.Item
          name='email'
          rules={[
            {
              type: 'email',
              message: 'The input is not valid E-mail!',
            },
            {
              required: true,
              message: 'Please input your E-mail!',
            },
          ]}
        >
          <Input
            value={email}
            prefix={<UserOutlined className='site-form-item-icon' />}
            placeholder='Email'
            onChange={(e) => setEmail(e.target.value)}
          />
        </Form.Item>

        <Form.Item
          name='password'
          rules={[
            {
              required: true,
              message: 'Please input your password!',
              pattern: regex,
            },
          ]}
          hasFeedback
        >
          <Input.Password
            value={password}
            prefix={<LockOutlined className='site-form-item-icon' />}
            placeholder='Password'
            onChange={(e) => {
              setPassword(e.target.value);
            }}
          />
        </Form.Item>

        <Form.Item
          name='confirm'
          dependencies={['password']}
          hasFeedback
          rules={[
            {
              required: true,
              message: 'Please confirm your password!',
            },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject(
                  new Error('The two passwords that you entered do not match!')
                );
              },
            }),
          ]}
        >
          <Input.Password
            placeholder='Confirm Password'
            prefix={<LockOutlined className='site-form-item-icon' />}
          />
        </Form.Item>

        <Form.Item>
          <Button type='primary' htmlType='submit'>
            Register
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
}

export default RegistrationForm;
