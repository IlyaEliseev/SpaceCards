import react from 'react';
import SignInForm from './SignInForm';

function SignInPages(props: { isModalVisible: boolean }) {
  return (
    <>
      <SignInForm isModalVisible={props.isModalVisible} />
    </>
  );
}
