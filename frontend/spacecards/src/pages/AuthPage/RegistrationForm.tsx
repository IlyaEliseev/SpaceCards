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
  const [userEmail, setUserEmail] = useState('');
  const [userPassword, setUserPassword] = useState('');
  const [userNickname, setUserNickname] = useState('');

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
      email: userEmail,
      nickname: userNickname,
      password: userPassword,
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
            value={userNickname}
            prefix={<UserOutlined className='site-form-item-icon' />}
            placeholder='Nickname'
            onChange={(e) => setUserNickname(e.target.value)}
          />
        </Form.Item>

        <Form.Item
          name='email'
          // label='E-mail'
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
            value={userEmail}
            prefix={<UserOutlined className='site-form-item-icon' />}
            placeholder='Email'
            onChange={(e) => setUserEmail(e.target.value)}
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
            value={userPassword}
            prefix={<LockOutlined className='site-form-item-icon' />}
            placeholder='Password'
            onChange={(e) => {
              setUserPassword(e.target.value);
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
          <Button
            // onClick={() => {
            //   onFinish();
            // }}
            type='primary'
            htmlType='submit'
          >
            Register
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
}

export default RegistrationForm;
