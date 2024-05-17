import { ChangeEvent, FC, useId } from 'react';
import './styles.scss';

interface UploadPathProps {
  className: string;
  labelName: string;
  path: string;
  setPath: (path: string) => void;
}

const UploadPath: FC<UploadPathProps> = ({
  className,
  labelName,
  path,
  setPath,
}) => {
  const id = useId();

  const handleSelectPath = (e: ChangeEvent<HTMLInputElement>) =>
    setPath(e.target.value);

  return (
    <div className={`upload-path ${className}`}>
      <div className="upload-path__frame upload-path__frame-label ">
        <label className="upload-path__label" htmlFor={id}>
          {labelName}
        </label>
      </div>
      <div className="upload-path__frame upload-path__frame-path ">
        <input
          className="upload-path__input"
          value={path}
          onChange={handleSelectPath}
          id={id}
          type={'text'}
        />
      </div>
    </div>
  );
};

export default UploadPath;
