import { Routes, Route } from 'react-router-dom';

import './styles.scss';
import TestSetting from '../../components/TestSetting';
import ContainerManagement from '../../components/ContainerManagement';
import LogInformation from '../../components/LogInformation';

const MainPage = () => {
  return (
    <div className="main-page">
      {/* <Header />
      <Routes>
        <Route path="*" element={<Main />} />
      </Routes> */}
      <div className="main-page__content-top">
        <TestSetting className="main-page__test-setting" />
        <ContainerManagement className="main-page__container-management" />
      </div>
      <div className="main-page__content-bottom">
        <LogInformation className="main-page__log-information" />
      </div>
    </div>
  );
};

export default MainPage;
