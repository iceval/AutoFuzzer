import { FC, useEffect } from 'react';
import './styles.scss';
import { fuzzerService } from '../../services/fuzzerService';
import { RefletionProjectProps } from '../../interfaces/Dtos/reflectionProject';

interface MethodTableProps {
  className: string;
  projectTestsPath: string;
  reflectionProject: RefletionProjectProps | undefined;
  setReflectionProject: React.Dispatch<
    React.SetStateAction<RefletionProjectProps | undefined>
  >;
}

const MethodTable: FC<MethodTableProps> = ({
  className,
  projectTestsPath,
  reflectionProject,
  setReflectionProject,
}) => {
  const getReflectionProject = () => {
    fuzzerService.getReflectionProjectAsync(projectTestsPath).then((data) => {
      setReflectionProject(data);
    });
  };

  useEffect(() => {
    getReflectionProject();
  }, [projectTestsPath]);

  console.log(reflectionProject);

  return (
    <div className={`method-table ${className}`}>
      <div className="method-table__method method-table__header">
        <div className="method-table__checkbox method-table__title">Исп.</div>
        <div className="method-table__method-name method-table__title">
          Название тестового метода
        </div>
      </div>
      <div className="method-table__method-list">
        {reflectionProject?.reflectionClasses?.map(
          (reflectionClass, classIndex) =>
            reflectionClass?.reflectionMethods?.map(
              (reflectionMethod, methodIndex) => (
                <div
                  key={reflectionMethod.methodName}
                  className="method-table__method"
                >
                  <div className="method-table__checkbox">
                    <input
                      type="checkbox"
                      className="method-table__checkbox-body"
                      onChange={() => {
                        setReflectionProject(
                          (
                            prevReflectionProject:
                              | RefletionProjectProps
                              | undefined
                          ) => {
                            if (prevReflectionProject == undefined)
                              return undefined;

                            const isSelectesd =
                              prevReflectionProject.reflectionClasses[
                                classIndex
                              ].reflectionMethods[methodIndex].isSelected;

                            prevReflectionProject.reflectionClasses[
                              classIndex
                            ].reflectionMethods[methodIndex].isSelected =
                              !isSelectesd;
                            return prevReflectionProject;
                          }
                        );
                        console.log(JSON.stringify(reflectionProject));
                      }}
                    />
                  </div>
                  <div className="method-table__method-name">
                    {reflectionClass.className}.{reflectionMethod.methodName}()
                  </div>
                </div>
              )
            )
        )}
      </div>
    </div>
  );
};

export default MethodTable;
