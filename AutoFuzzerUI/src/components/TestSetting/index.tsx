import { FC, useState } from 'react';
import './styles.scss';
import WindowForm from '../WindowForm';
import UploadPath from '../UploadPath';
import MethodTable from '../MethodTable';
import { RefletionProjectProps } from '../../interfaces/Dtos/reflectionProject';
import { fuzzerService } from '../../services/fuzzerService';

interface TestSettingProps {
  className: string;
}

const TestSetting: FC<TestSettingProps> = ({ className }) => {
  const [projectPath, setProjectPath] = useState<string>('');
  const [projectTestsPath, setProjectTestsPath] = useState<string>('');
  const [reflectionProject, setReflectionProject] = useState<
    RefletionProjectProps | undefined
  >();

  const run = () => {
    fuzzerService.runAsync(projectTestsPath, projectPath, reflectionProject);
  };

  return (
    <div className={`test-setting ${className}`}>
      <WindowForm
        className="test-setting__body"
        title="Настройка тестов фаззера"
      >
        <UploadPath
          className="test-setting__upload-path"
          labelName="Путь проекта библиотеки"
          path={projectPath}
          setPath={setProjectPath}
        />
        <UploadPath
          className="test-setting__upload-path"
          labelName="Путь проекта c тестами"
          path={projectTestsPath}
          setPath={setProjectTestsPath}
        />
        <MethodTable
          className="test-setting__method-table"
          projectTestsPath={projectTestsPath}
          reflectionProject={reflectionProject}
          setReflectionProject={setReflectionProject}
        />
        <button className="test-setting__button" onClick={run}>
          Начать
        </button>
      </WindowForm>
    </div>
  );
};

export default TestSetting;
