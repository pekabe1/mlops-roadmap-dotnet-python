using System.Collections.Generic;

namespace MLOpsRoadmap.Api;

public record CourseModule(
    string Id,
    string Title,
    string Emoji,
    string Description,
    int Order,
    List<string> Tags,
    List<string> Tasks
);

public record CourseModuleDetail(
    string Id,
    string Title,
    string Emoji,
    string Description,
    int Order,
    List<string> Tags,
    List<string> Tasks,
    string Content
);

public class CourseService
{
    private static readonly List<CourseModule> Modules = new()
    {
        new("intro", "Introduction to MLOps", "\U0001F3AF",
            "Learn who MLOps practitioners are, how DevOps differs from MLOps, and what skills transfer from .NET.",
            1, ["intro", "concepts"],
            ["Understand MLOps vs DevOps differences", "Identify transferable .NET skills", "Set up learning environment"]),

        new("python-fundamentals", "Python Fundamentals for .NET Developers", "\U0001F40D",
            "Master Python syntax, type hints, virtual environments, and essential libraries from a .NET perspective.",
            2, ["python", "basics"],
            ["Compare C# vs Python syntax", "Create and manage virtual environments", "Learn NumPy and Pandas basics", "Write pytest tests"]),

        new("ml-basics", "Machine Learning Basics", "\U0001F916",
            "Learn core ML concepts, scikit-learn, model training, evaluation metrics, and feature engineering.",
            3, ["ml", "scikit-learn", "python"],
            ["Understand supervised vs unsupervised learning", "Train first model with scikit-learn", "Evaluate model performance", "Perform feature engineering"]),

        new("mlops-tools", "MLOps Tools & Practices", "\U0001F680",
            "Learn MLflow, DVC, model versioning, experiment tracking, and ML pipelines.",
            4, ["mlops", "mlflow", "dvc"],
            ["Set up MLflow tracking", "Version data with DVC", "Build ML pipelines", "Track experiments"]),

        new("data-engineering", "Data Engineering", "\U0001F4CA",
            "Learn Pandas for data manipulation, DVC for data versioning, and data pipeline best practices.",
            5, ["pandas", "dvc", "data"],
            ["Load and clean datasets with Pandas", "Version datasets with DVC", "Build data preprocessing pipelines", "Handle missing data"]),

        new("model-training", "Model Training", "\U0001F9E0",
            "Deep dive into training ML models using scikit-learn and PyTorch, hyperparameter tuning, and experiment management.",
            6, ["pytorch", "scikit-learn", "training"],
            ["Train models with scikit-learn", "Build neural networks with PyTorch", "Tune hyperparameters with Optuna", "Log experiments with MLflow"]),

        new("model-serving", "Model Serving & Deployment", "\U0001F310",
            "Deploy ML models using ASP.NET Core with ML.NET or ONNX Runtime, FastAPI, and containerization.",
            7, ["dotnet", "aspnet", "onnx", "deployment"],
            ["Create prediction API with ASP.NET Core + ML.NET", "Export model to ONNX format", "Call ONNX model from .NET", "Containerize with Docker"]),

        new("cicd", "CI/CD for ML", "\u2699\uFE0F",
            "Build automated ML pipelines using GitHub Actions, automated model testing, and deployment workflows.",
            8, ["github-actions", "cicd", "automation"],
            ["Create GitHub Actions workflow for ML training", "Add automated model quality checks", "Set up staging and production deployments", "Monitor model drift"]),

        new("monitoring", "Model Monitoring", "\U0001F4C8",
            "Learn to monitor deployed models, detect data drift, and set up alerts and retraining pipelines.",
            9, ["monitoring", "drift", "production"],
            ["Set up model performance monitoring", "Detect and handle data drift", "Create alerting rules", "Automate model retraining"]),

        new("platform", "Platform & Infrastructure", "\u2601\uFE0F",
            "Learn Kubernetes, Azure ML, and infrastructure-as-code patterns for ML workloads.",
            10, ["kubernetes", "azure", "infrastructure"],
            ["Deploy ML workloads to Kubernetes", "Use Azure ML workspace", "Write infrastructure as code with Terraform", "Implement resource monitoring"]),
    };

    private static string GetContent(string id) => id switch
    {
        "intro" => IntroContent,
        "python-fundamentals" => PythonContent,
        "ml-basics" => MlBasicsContent,
        "mlops-tools" => MlopsToolsContent,
        "data-engineering" => DataEngContent,
        "model-training" => ModelTrainingContent,
        "model-serving" => ModelServingContent,
        "cicd" => CicdContent,
        "monitoring" => MonitoringContent,
        "platform" => PlatformContent,
        _ => "# Content Coming Soon"
    };

    public List<CourseModule> GetModules() => Modules;

    public CourseModuleDetail? GetModuleDetail(string id)
    {
        var module = Modules.FirstOrDefault(m => m.Id == id);
        if (module is null) return null;
        return new CourseModuleDetail(
            module.Id, module.Title, module.Emoji, module.Description,
            module.Order, module.Tags, module.Tasks, GetContent(id));
    }

    private const string IntroContent = @"# Introduction to MLOps

## What is MLOps?

MLOps (Machine Learning Operations) is the discipline of deploying, monitoring,
and maintaining ML models in production. As a .NET developer, you already understand
software lifecycle management — MLOps extends these principles to ML systems.

### MLOps vs DevOps

| Aspect | DevOps | MLOps |
|--------|--------|-------|
| Artifact | Code/Binary | Model + Code + Data |
| Testing | Unit, Integration | Data validation, Model evaluation |
| Versioning | Git | Git + DVC + Model Registry |
| CI/CD triggers | Code commit | Code commit OR data change OR drift |
| Monitoring | Uptime, latency | Accuracy, data drift, model decay |

## Why MLOps Matters

ML systems can degrade silently. A deployed model becomes less accurate over time
due to **data drift** — the real-world distribution shifts from training data.
Without MLOps practices, you would never know until users complain.

## Skills Transfer from .NET

- **C# -> Python**: Similar OOP concepts, LINQ -> list comprehensions
- **NuGet -> pip/conda**: Package management
- **ASP.NET Core -> FastAPI**: REST API development
- **xUnit/NUnit -> pytest**: Testing frameworks
- **Azure DevOps -> GitHub Actions**: CI/CD pipelines
- **Entity Framework -> Pandas/SQLAlchemy**: Data access

## The MLOps Maturity Model

```
Level 0: Manual process (Jupyter notebooks, manual deployments)
Level 1: ML pipeline automation (automated training, feature stores)
Level 2: CI/CD pipeline automation (automated deployment, monitoring)
```

## Setting Up Your Environment

```bash
# Create a virtual environment
python -m venv mlops-env
source mlops-env/bin/activate   # Linux/Mac
# mlops-env\Scripts\activate   # Windows

# Install core packages
pip install numpy pandas scikit-learn matplotlib jupyter mlflow

# Verify
python -c ""import sklearn; print(sklearn.__version__)""
```

For .NET integration:

```bash
dotnet add package Microsoft.ML
dotnet add package Microsoft.ML.OnnxRuntime
```

## Next Steps

- Complete the Python Fundamentals module to bridge your C# knowledge
- Set up Jupyter Lab for interactive exploration
- Track your progress through all 10 modules
";

    private const string PythonContent = @"# Python Fundamentals for .NET Developers

## Syntax Comparison: C# vs Python

### Variables and Types

```csharp
// C#
string name = ""Alice"";
int age = 30;
double score = 9.5;
bool isActive = true;
var items = new List<string> { ""a"", ""b"", ""c"" };
```

```python
# Python
name: str = ""Alice""
age: int = 30
score: float = 9.5
is_active: bool = True
items: list[str] = [""a"", ""b"", ""c""]
```

### Functions

```csharp
// C#
public static double CalculateRmse(double[] predictions, double[] actuals)
{
    return Math.Sqrt(predictions.Zip(actuals)
        .Average(p => Math.Pow(p.First - p.Second, 2)));
}
```

```python
# Python
import numpy as np

def calculate_rmse(predictions: list[float], actuals: list[float]) -> float:
    errors = np.array(predictions) - np.array(actuals)
    return float(np.sqrt(np.mean(errors ** 2)))
```

### Classes

```csharp
// C#
public class ModelTrainer
{
    private readonly string _modelName;
    public ModelTrainer(string modelName) { _modelName = modelName; }
    public void Train(IEnumerable<DataPoint> data) { /* ... */ }
}
```

```python
# Python
from dataclasses import dataclass

@dataclass
class ModelTrainer:
    model_name: str

    def train(self, data: list) -> None:
        pass
```

## Virtual Environments

```bash
# Create
python -m venv .venv

# Activate
source .venv/bin/activate       # Linux/Mac
.venv\Scripts\activate          # Windows

# Manage packages
pip install numpy pandas scikit-learn
pip freeze > requirements.txt
pip install -r requirements.txt
```

## NumPy Basics

```python
import numpy as np

arr = np.array([1, 2, 3, 4, 5])
matrix = np.array([[1, 2, 3], [4, 5, 6]])

# Vectorized operations (no explicit loops)
squared = arr ** 2           # [1, 4, 9, 16, 25]
normalized = (arr - arr.mean()) / arr.std()

print(f""Shape: {matrix.shape}"")     # (2, 3)
print(f""Mean:  {arr.mean():.2f}"")   # 3.00
```

## Pandas Basics

```python
import pandas as pd

df = pd.read_csv(""data.csv"")

# Filter (.Where() equivalent)
adults = df[df[""age""] > 18]

# Transform (.Select() equivalent)
df = df.assign(age_squared=df[""age""] ** 2)

# Aggregate (.GroupBy() equivalent)
avg_by_dept = df.groupby(""department"")[""salary""].mean()

# Sort (.OrderBy() equivalent)
sorted_df = df.sort_values(""salary"", ascending=False)

print(df.describe())
```

## Testing with pytest

```python
# test_calculator.py
import pytest
from calculator import calculate_rmse

def test_rmse_perfect_predictions():
    assert calculate_rmse([1.0, 2.0], [1.0, 2.0]) == 0.0

@pytest.mark.parametrize(""pred,actual,expected"", [
    ([2.0], [4.0], 2.0),
    ([1.0, 3.0], [3.0, 1.0], 2.0),
])
def test_rmse_parametrized(pred, actual, expected):
    assert abs(calculate_rmse(pred, actual) - expected) < 1e-9
```

```bash
pytest tests/ -v --cov=src --cov-report=html
```

## Next Steps

- Rewrite a simple C# utility in Python for practice
- Explore Jupyter notebooks for interactive data exploration
- Move on to Machine Learning Basics using your new Python skills
";

    private const string MlBasicsContent = @"# Machine Learning Basics

## Core Concepts

Machine learning enables computers to learn from data instead of explicit rules.
As a .NET developer, think of ML as building functions where the logic is
*learned* from examples rather than hand-coded.

### Types of Learning

**Supervised Learning** — labeled input/output pairs:
- Classification: predict a category (spam, disease)
- Regression: predict a number (price, temperature)

**Unsupervised Learning** — find structure without labels:
- Clustering: customer segments
- Dimensionality reduction: PCA, t-SNE

## scikit-learn Basics

```python
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.datasets import load_iris

X, y = load_iris(return_X_y=True)
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Always fit scaler on training data only
scaler = StandardScaler()
X_train_s = scaler.fit_transform(X_train)
X_test_s  = scaler.transform(X_test)

model = RandomForestClassifier(n_estimators=100, random_state=42)
model.fit(X_train_s, y_train)
predictions = model.predict(X_test_s)
```

## Evaluation Metrics

```python
from sklearn.metrics import accuracy_score, f1_score, classification_report

acc = accuracy_score(y_test, predictions)
f1  = f1_score(y_test, predictions, average=""weighted"")

print(f""Accuracy: {acc:.4f}"")
print(f""F1 Score: {f1:.4f}"")
print(classification_report(y_test, predictions))
```

## Feature Engineering

```python
import pandas as pd
from sklearn.impute import SimpleImputer

data = pd.DataFrame({
    ""age"":    [25, None, 35, 22, 45],
    ""income"": [50000, 75000, None, 40000, 90000],
    ""city"":   [""NY"", ""LA"", ""NY"", ""SF"", ""LA""],
})

# Fill missing values
imputer = SimpleImputer(strategy=""median"")
data[[""age"", ""income""]] = imputer.fit_transform(data[[""age"", ""income""]])

# One-hot encode categories
data = pd.get_dummies(data, columns=[""city""], prefix=""city"")
print(data.head())
```

## Cross-Validation

```python
from sklearn.model_selection import cross_val_score, StratifiedKFold

cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
scores = cross_val_score(
    RandomForestClassifier(n_estimators=100),
    X, y, cv=cv, scoring=""accuracy""
)
print(f""CV Accuracy: {scores.mean():.4f} +/- {scores.std():.4f}"")
```

## Pipelines — Prevent Data Leakage

```python
from sklearn.pipeline import Pipeline

pipeline = Pipeline([
    (""scaler"",     StandardScaler()),
    (""classifier"", RandomForestClassifier(n_estimators=100))
])

pipeline.fit(X_train, y_train)
score = pipeline.score(X_test, y_test)
print(f""Pipeline accuracy: {score:.4f}"")
```

## Next Steps

- Explore MLOps Tools to track your experiments with MLflow
- Try a Kaggle beginner competition to apply these skills
- Learn deep learning with PyTorch in the Model Training module
";

    private const string MlopsToolsContent = @"# MLOps Tools & Practices

## MLflow: Experiment Tracking

MLflow is the industry standard for tracking ML experiments — think of it as
a specialized Git for ML runs, complete with a web UI.

```bash
pip install mlflow
mlflow ui   # Opens http://localhost:5000
```

### Tracking an Experiment

```python
import mlflow, mlflow.sklearn
from sklearn.ensemble import RandomForestClassifier
from sklearn.datasets import load_iris
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score

mlflow.set_experiment(""iris-classification"")

X, y = load_iris(return_X_y=True)
X_tr, X_te, y_tr, y_te = train_test_split(X, y, test_size=0.2, random_state=42)

with mlflow.start_run(run_name=""rf-v1""):
    n_est, depth = 100, 5
    mlflow.log_params({""n_estimators"": n_est, ""max_depth"": depth})

    model = RandomForestClassifier(n_estimators=n_est, max_depth=depth)
    model.fit(X_tr, y_tr)

    acc = accuracy_score(y_te, model.predict(X_te))
    mlflow.log_metric(""accuracy"", acc)
    mlflow.sklearn.log_model(model, ""model"", registered_model_name=""IrisClassifier"")
    print(f""Accuracy: {acc:.4f}"")
```

### Loading a Registered Model

```python
import mlflow.sklearn

# By run ID
model = mlflow.sklearn.load_model(f""runs:/{run_id}/model"")

# By name and stage
model = mlflow.sklearn.load_model(""models:/IrisClassifier/Production"")
```

## DVC: Data Version Control

DVC extends Git to handle large datasets and binary files.

```bash
git init && dvc init

dvc add data/raw/dataset.csv
git add data/raw/dataset.csv.dvc .gitignore
git commit -m ""Track raw dataset with DVC""

dvc remote add -d myremote s3://my-bucket/dvc
dvc push    # Upload data to remote
dvc pull    # Restore data on another machine
```

### DVC Pipeline (dvc.yaml)

```yaml
stages:
  prepare:
    cmd: python src/prepare.py
    deps:
      - src/prepare.py
      - data/raw/dataset.csv
    outs:
      - data/processed/train.csv

  train:
    cmd: python src/train.py
    deps:
      - src/train.py
      - data/processed/train.csv
    params:
      - params.yaml:
        - model.n_estimators
    outs:
      - models/model.pkl
    metrics:
      - metrics/scores.json:
          cache: false
```

```bash
dvc repro         # Reproduce pipeline
dvc dag           # Visualize pipeline graph
dvc metrics show  # Display metrics
dvc metrics diff  # Compare between commits
```

## Model Registry Workflow

```python
from mlflow.tracking import MlflowClient

client = MlflowClient()

client.transition_model_version_stage(""IrisClassifier"", version=1, stage=""Staging"")
# ... run integration tests ...
client.transition_model_version_stage(""IrisClassifier"", version=1, stage=""Production"")
```

## Next Steps

- Set up a shared MLflow tracking server for your team
- Integrate DVC with your CI/CD pipeline
- Explore Data Engineering to manage dataset versioning at scale
";

    private const string DataEngContent = @"# Data Engineering for ML

## Data Loading and Exploration

Clean data beats sophisticated algorithms. Invest time here.

```python
import pandas as pd
import numpy as np

df = pd.read_csv(""data.csv"")
df_parquet = pd.read_parquet(""data.parquet"")  # Faster for large datasets

# Initial exploration
print(df.shape)
print(df.dtypes)
print(df.describe())
print(""Missing:"", df.isnull().sum())
print(""Duplicates:"", df.duplicated().sum())
```

## Data Cleaning

```python
df = df.copy()

# Fill missing values by strategy
df[""age""].fillna(df[""age""].median(), inplace=True)
df[""category""].fillna(df[""category""].mode()[0], inplace=True)

# Remove duplicates
df = df.drop_duplicates(subset=[""id""], keep=""first"")

# Fix data types
df[""date""]  = pd.to_datetime(df[""date""])
df[""price""] = pd.to_numeric(df[""price""], errors=""coerce"")

# Remove outliers using IQR
Q1, Q3 = df[""price""].quantile([0.25, 0.75])
IQR = Q3 - Q1
df = df[df[""price""].between(Q1 - 1.5*IQR, Q3 + 1.5*IQR)]
```

## Feature Engineering Pipeline

```python
from sklearn.pipeline import Pipeline
from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import StandardScaler, OneHotEncoder
from sklearn.impute import SimpleImputer

numeric_features     = [""age"", ""income"", ""score""]
categorical_features = [""city"", ""category""]

num_transformer = Pipeline([
    (""imputer"", SimpleImputer(strategy=""median"")),
    (""scaler"",  StandardScaler())
])

cat_transformer = Pipeline([
    (""imputer"", SimpleImputer(strategy=""most_frequent"")),
    (""onehot"",  OneHotEncoder(handle_unknown=""ignore"", sparse_output=False))
])

preprocessor = ColumnTransformer([
    (""num"", num_transformer, numeric_features),
    (""cat"", cat_transformer, categorical_features)
])
```

## Versioning Data with DVC

```bash
git init && dvc init
mkdir -p data/raw data/processed
dvc add data/raw/customers.csv
```

```python
# src/process_data.py
import pandas as pd
import numpy as np
import sys

def process(input_path: str, output_path: str) -> None:
    df = pd.read_csv(input_path)
    df = df.dropna(subset=[""target""])
    df[""age""]        = df[""age""].clip(0, 120)
    df[""log_income""] = np.log1p(df[""income""])
    df.to_csv(output_path, index=False)
    print(f""Processed {len(df)} rows -> {output_path}"")

if __name__ == ""__main__"":
    process(sys.argv[1], sys.argv[2])
```

```bash
dvc run -n process_data \
    -d src/process_data.py \
    -d data/raw/customers.csv \
    -o data/processed/customers_clean.csv \
    python src/process_data.py data/raw/customers.csv data/processed/customers_clean.csv
```

## Next Steps

- Add data validation (Great Expectations, Pandera) to your pipeline
- Explore feature stores like Feast for production use
- Move on to Model Training to use your prepared data
";

    private const string ModelTrainingContent = @"# Model Training

## Full Training Pipeline with scikit-learn + MLflow

```python
import pandas as pd
import mlflow, mlflow.sklearn
from sklearn.ensemble import GradientBoostingClassifier
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import roc_auc_score, classification_report
import joblib

df = pd.read_csv(""data/processed/train.csv"")
X, y = df.drop(""target"", axis=1), df[""target""]
X_tr, X_te, y_tr, y_te = train_test_split(X, y, test_size=0.2, stratify=y)

mlflow.set_experiment(""model-training"")

with mlflow.start_run():
    param_grid = {
        ""clf__n_estimators"": [100, 200],
        ""clf__max_depth"":    [3, 5, 7],
        ""clf__learning_rate"":[0.05, 0.1],
    }

    pipe = Pipeline([(""scaler"", StandardScaler()), (""clf"", GradientBoostingClassifier())])
    gs   = GridSearchCV(pipe, param_grid, cv=5, scoring=""roc_auc"", n_jobs=-1)
    gs.fit(X_tr, y_tr)

    best = gs.best_estimator_
    auc  = roc_auc_score(y_te, best.predict_proba(X_te)[:, 1])

    mlflow.log_params(gs.best_params_)
    mlflow.log_metric(""roc_auc"", auc)
    mlflow.sklearn.log_model(best, ""model"")
    joblib.dump(best, ""models/best_model.pkl"")
    print(f""ROC-AUC: {auc:.4f}"")
    print(classification_report(y_te, best.predict(X_te)))
```

## PyTorch Neural Networks

```python
import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader, TensorDataset

class MLPClassifier(nn.Module):
    def __init__(self, input_dim: int, hidden_dims: list[int], dropout: float = 0.3):
        super().__init__()
        layers, prev = [], input_dim
        for h in hidden_dims:
            layers += [nn.Linear(prev, h), nn.BatchNorm1d(h), nn.ReLU(), nn.Dropout(dropout)]
            prev = h
        layers += [nn.Linear(prev, 1), nn.Sigmoid()]
        self.net = nn.Sequential(*layers)

    def forward(self, x: torch.Tensor) -> torch.Tensor:
        return self.net(x).squeeze(1)

device = torch.device(""cuda"" if torch.cuda.is_available() else ""cpu"")
model  = MLPClassifier(input_dim=20, hidden_dims=[128, 64, 32]).to(device)

optimizer = optim.AdamW(model.parameters(), lr=1e-3, weight_decay=1e-4)
criterion = nn.BCELoss()
scheduler = optim.lr_scheduler.ReduceLROnPlateau(optimizer, patience=5)

for epoch in range(100):
    model.train()
    # ... batch training loop ...
    if epoch % 10 == 0:
        print(f""Epoch {epoch}: training..."")
```

## Hyperparameter Optimization with Optuna

```python
import optuna
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import cross_val_score

def objective(trial) -> float:
    params = {
        ""n_estimators"": trial.suggest_int(""n_estimators"", 50, 500),
        ""max_depth"":    trial.suggest_int(""max_depth"", 2, 20),
        ""max_features"": trial.suggest_categorical(""max_features"", [""sqrt"", ""log2""]),
    }
    model  = RandomForestClassifier(**params, random_state=42)
    scores = cross_val_score(model, X_tr, y_tr, cv=3, scoring=""roc_auc"")
    return scores.mean()

study = optuna.create_study(direction=""maximize"")
study.optimize(objective, n_trials=50, timeout=300)
print(f""Best AUC: {study.best_value:.4f}"")
print(f""Best params: {study.best_params}"")
```

## Next Steps

- Export your trained model to ONNX for .NET deployment
- Set up automated retraining pipelines in the CI/CD module
- Move on to Model Serving to put your model in production
";

    private const string ModelServingContent = @"# Model Serving & Deployment

## ASP.NET Core + ML.NET

```csharp
// MLPredictionService.cs
using Microsoft.ML;
using Microsoft.ML.Data;

public class HouseData   { public float Size { get; set; } }
public class HouseResult { [ColumnName(""Score"")] public float Price { get; set; } }

public class MLPredictionService
{
    private readonly PredictionEngine<HouseData, HouseResult> _engine;

    public MLPredictionService(IWebHostEnvironment env)
    {
        var ctx   = new MLContext();
        var path  = Path.Combine(env.ContentRootPath, ""models"", ""house_model.zip"");
        var model = ctx.Model.Load(path, out _);
        _engine   = ctx.Model.CreatePredictionEngine<HouseData, HouseResult>(model);
    }

    public float Predict(float size) => _engine.Predict(new HouseData { Size = size }).Price;
}

// Program.cs
// builder.Services.AddSingleton<MLPredictionService>();
// app.MapPost(""/predict"", (PredictReq r, MLPredictionService s) =>
//     Results.Ok(new { price = s.Predict(r.Size) }));
```

## ONNX Runtime — Train in Python, Serve in .NET

```python
# Python: export model to ONNX
from skl2onnx import convert_sklearn
from skl2onnx.common.data_types import FloatTensorType

initial_type = [(""float_input"", FloatTensorType([None, X_train.shape[1]]))]
onnx_model   = convert_sklearn(model, initial_types=initial_type)

with open(""model.onnx"", ""wb"") as f:
    f.write(onnx_model.SerializeToString())
```

```csharp
// C#: run the ONNX model
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

public class OnnxService
{
    private readonly InferenceSession _session;

    public OnnxService(string modelPath) => _session = new InferenceSession(modelPath);

    public float[] Predict(float[][] data)
    {
        var flat   = data.SelectMany(r => r).ToArray();
        var tensor = new DenseTensor<float>(flat, new[] { data.Length, data[0].Length });
        var inputs = new[] { NamedOnnxValue.CreateFromTensor(""float_input"", tensor) };
        using var res = _session.Run(inputs);
        return res.First().AsEnumerable<float>().ToArray();
    }
}
```

## FastAPI Python Serving

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import joblib, numpy as np, uvicorn

app = FastAPI(title=""ML Model API"", version=""1.0.0"")

@app.on_event(""startup"")
async def load_model():
    app.state.model  = joblib.load(""models/model.pkl"")
    app.state.scaler = joblib.load(""models/scaler.pkl"")

class PredictRequest(BaseModel):
    features: list[float]

class PredictResponse(BaseModel):
    prediction:  int
    probability: float

@app.post(""/predict"", response_model=PredictResponse)
async def predict(req: PredictRequest):
    X      = np.array(req.features).reshape(1, -1)
    X_s    = app.state.scaler.transform(X)
    pred   = int(app.state.model.predict(X_s)[0])
    prob   = float(app.state.model.predict_proba(X_s)[0].max())
    return PredictResponse(prediction=pred, probability=prob)

@app.get(""/health"")
async def health():
    return {""status"": ""ok""}
```

## Dockerfile

```dockerfile
FROM python:3.11-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY src/ ./src/
COPY models/ ./models/
EXPOSE 8000
CMD [""uvicorn"", ""src.api:app"", ""--host"", ""0.0.0.0"", ""--port"", ""8000""]
```

## Next Steps

- Automate model deployment with GitHub Actions (CI/CD module)
- Add Prometheus metrics to your FastAPI server
- Move on to Model Monitoring to detect degradation in production
";

    private const string CicdContent = @"# CI/CD for Machine Learning

## GitHub Actions ML Pipeline

CI/CD for ML adds data validation, model training, and quality gates to standard CI.

```yaml
# .github/workflows/ml-pipeline.yml
name: ML Training Pipeline

on:
  push:
    branches: [main]
    paths: ['src/**', 'data/**/*.dvc', 'params.yaml']
  workflow_dispatch:

jobs:
  data-validation:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with: { python-version: '3.11', cache: pip }
      - run: pip install -r requirements.txt
      - run: python src/validate_data.py

  train:
    needs: data-validation
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with: { python-version: '3.11', cache: pip }
      - run: pip install -r requirements.txt
      - run: dvc repro
      - name: Report metrics
        run: |
          echo ""## Model Metrics"" >> $GITHUB_STEP_SUMMARY
          cat metrics/scores.json >> $GITHUB_STEP_SUMMARY
      - uses: actions/upload-artifact@v4
        with: { name: trained-model, path: models/ }

  quality-gate:
    needs: train
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/download-artifact@v4
        with: { name: trained-model, path: models/ }
      - name: Check model quality
        run: |
          python src/quality_gate.py --accuracy-threshold 0.85 --auc-threshold 0.90

  deploy-staging:
    needs: quality-gate
    runs-on: ubuntu-latest
    environment: staging
    if: github.ref == 'refs/heads/main'
    steps:
      - uses: actions/checkout@v4
      - name: Build and push image
        run: |
          docker build -t ghcr.io/${{ github.repository }}/ml-api:${{ github.sha }} .
          docker push  ghcr.io/${{ github.repository }}/ml-api:${{ github.sha }}
```

## Automated Model Quality Gate

```python
# src/quality_gate.py
import argparse, json, sys

parser = argparse.ArgumentParser()
parser.add_argument(""--accuracy-threshold"", type=float, default=0.85)
parser.add_argument(""--auc-threshold"",      type=float, default=0.90)
args = parser.parse_args()

with open(""metrics/test_scores.json"") as f:
    metrics = json.load(f)

thresholds = {
    ""accuracy"": args.accuracy_threshold,
    ""roc_auc"":  args.auc_threshold,
}

failed = []
for metric, threshold in thresholds.items():
    value = metrics.get(metric, 0.0)
    status = ""PASS"" if value >= threshold else ""FAIL""
    print(f""[{status}] {metric}: {value:.4f} (threshold: {threshold})"")
    if value < threshold:
        failed.append(metric)

if failed:
    print(f""\nQuality gate FAILED for: {', '.join(failed)}"")
    sys.exit(1)

print(""\nQuality gate PASSED"")
```

## Automated Model Testing

```python
# tests/test_model_quality.py
import pytest, joblib, numpy as np, pandas as pd
from sklearn.metrics import accuracy_score, roc_auc_score

@pytest.fixture(scope=""session"")
def model():
    return joblib.load(""models/model.pkl"")

@pytest.fixture(scope=""session"")
def test_data():
    df = pd.read_csv(""data/processed/test.csv"")
    return df.drop(""target"", axis=1), df[""target""]

def test_accuracy(model, test_data):
    X, y = test_data
    acc  = accuracy_score(y, model.predict(X))
    assert acc >= 0.85, f""Accuracy {acc:.4f} below 0.85""

def test_roc_auc(model, test_data):
    X, y = test_data
    auc  = roc_auc_score(y, model.predict_proba(X)[:, 1])
    assert auc >= 0.90, f""ROC-AUC {auc:.4f} below 0.90""

def test_prediction_latency(model, test_data):
    import time
    X, _ = test_data
    t0   = time.time()
    model.predict(X[:100])
    assert time.time() - t0 < 1.0, ""Prediction too slow""
```

## Automated Retraining

```yaml
# .github/workflows/retrain-on-drift.yml
name: Automated Retraining
on:
  schedule:
    - cron: '0 2 * * *'

jobs:
  check-drift:
    runs-on: ubuntu-latest
    outputs:
      drift: ${{ steps.check.outputs.drift }}
    steps:
      - uses: actions/checkout@v4
      - id: check
        run: echo ""drift=$(python src/check_drift.py)"" >> $GITHUB_OUTPUT

  retrain:
    needs: check-drift
    if: needs.check-drift.outputs.drift == 'true'
    uses: ./.github/workflows/ml-pipeline.yml
```

## Next Steps

- Integrate model monitoring alerts into your CI/CD pipeline
- Implement blue/green or canary deployments for safer rollouts
- Move on to Model Monitoring to close the MLOps loop
";

    private const string MonitoringContent = @"# Model Monitoring

## Why Models Degrade in Production

Unlike traditional software, ML models can silently become less accurate as
the real-world data distribution drifts from the training distribution.

### Types of Drift

| Type | Description |
|------|-------------|
| Data drift (covariate shift) | Input feature distribution changes |
| Concept drift | Relationship between features and target changes |
| Label drift | Target variable distribution changes |

## Drift Detection with Evidently

```python
# monitoring/drift_detector.py
import pandas as pd
from evidently.report import Report
from evidently.metric_preset import DataDriftPreset, DataQualityPreset

def detect_drift(reference: pd.DataFrame, current: pd.DataFrame) -> dict:
    report = Report(metrics=[DataDriftPreset(), DataQualityPreset()])
    report.run(reference_data=reference, current_data=current)
    report.save_html(""reports/drift_report.html"")

    result    = report.as_dict()
    drift_res = result[""metrics""][0][""result""]
    return {
        ""drift_detected"": drift_res[""dataset_drift""],
        ""drift_share"":    drift_res[""drift_share""],
    }

def run_monitoring_job() -> dict:
    import mlflow

    reference = pd.read_csv(""data/processed/train.csv"")
    current   = pd.read_csv(""data/production/recent.csv"")

    result = detect_drift(reference, current)

    with mlflow.start_run(run_name=""drift-check""):
        mlflow.log_metric(""drift_share"", result[""drift_share""])

    if result[""drift_detected""]:
        trigger_retraining_pipeline()

    return result
```

## Performance Tracking

```python
# monitoring/tracker.py
import sqlite3
from datetime import datetime, timedelta
from sklearn.metrics import accuracy_score

class PerformanceTracker:
    def __init__(self, db_path: str = ""monitoring.db""):
        self.db_path = db_path
        self._init_db()

    def _init_db(self):
        with sqlite3.connect(self.db_path) as conn:
            conn.execute(
                ""CREATE TABLE IF NOT EXISTS predictions ""
                ""(id TEXT PRIMARY KEY, ts TEXT, prediction INTEGER, ""
                "" probability REAL, actual INTEGER, version TEXT)""
            )

    def log_prediction(self, pred_id: str, prediction: int,
                       probability: float, version: str = ""1.0.0""):
        with sqlite3.connect(self.db_path) as conn:
            conn.execute(
                ""INSERT OR REPLACE INTO predictions VALUES (?,?,?,?,NULL,?)"",
                (pred_id, datetime.utcnow().isoformat(), prediction, probability, version)
            )

    def get_accuracy(self, hours: int = 24) -> float | None:
        since = (datetime.utcnow() - timedelta(hours=hours)).isoformat()
        with sqlite3.connect(self.db_path) as conn:
            rows = conn.execute(
                ""SELECT prediction, actual FROM predictions ""
                ""WHERE ts >= ? AND actual IS NOT NULL"", (since,)
            ).fetchall()
        if not rows:
            return None
        return accuracy_score([r[1] for r in rows], [r[0] for r in rows])
```

## Prometheus Metrics

```python
from prometheus_client import Counter, Gauge, Histogram, start_http_server

PREDICTIONS_TOTAL = Counter(
    ""ml_predictions_total"", ""Total predictions"", [""version""]
)
MODEL_ACCURACY = Gauge(
    ""ml_model_accuracy"", ""Rolling accuracy"", [""window_hours""]
)
PREDICTION_LATENCY = Histogram(
    ""ml_prediction_latency_seconds"", ""Prediction latency"",
    buckets=[0.001, 0.005, 0.01, 0.05, 0.1, 0.5, 1.0]
)

start_http_server(9090)
```

## Next Steps

- Set up Grafana dashboards connected to Prometheus
- Integrate drift detection into nightly CI/CD checks
- Move on to Platform & Infrastructure for production deployment
";

    private const string PlatformContent = @"# Platform & Infrastructure for ML

## Kubernetes Deployment

```yaml
# k8s/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ml-api
spec:
  replicas: 3
  selector:
    matchLabels: { app: ml-api }
  template:
    metadata:
      labels: { app: ml-api }
    spec:
      containers:
        - name: ml-api
          image: ghcr.io/myorg/ml-api:v1.2.3
          ports:
            - containerPort: 8000
          resources:
            requests: { memory: 256Mi, cpu: 250m }
            limits:   { memory: 512Mi, cpu: 500m }
          livenessProbe:
            httpGet: { path: /health, port: 8000 }
            initialDelaySeconds: 15
          readinessProbe:
            httpGet: { path: /health, port: 8000 }
            initialDelaySeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: ml-api-svc
spec:
  selector: { app: ml-api }
  ports:
    - port: 80
      targetPort: 8000
```

## Azure ML Workspace

```python
from azure.ai.ml import MLClient, command
from azure.ai.ml.entities import AmlCompute
from azure.identity import DefaultAzureCredential

ml_client = MLClient(
    credential=DefaultAzureCredential(),
    subscription_id=""your-subscription-id"",
    resource_group_name=""your-rg"",
    workspace_name=""your-workspace""
)

# Create a training compute cluster
compute = AmlCompute(
    name=""training-cluster"",
    size=""Standard_DS3_v2"",
    min_instances=0,
    max_instances=4,
    idle_time_before_scale_down=120
)
ml_client.begin_create_or_update(compute).result()

# Submit a training job
job = command(
    code=""./src"",
    command=""python train.py"",
    environment=""ml-env@latest"",
    compute=""training-cluster"",
    experiment_name=""iris-classification""
)
returned_job = ml_client.jobs.create_or_update(job)
ml_client.jobs.stream(returned_job.name)
```

## Terraform Infrastructure as Code

```hcl
# main.tf
terraform {
  required_providers {
    azurerm = { source = ""hashicorp/azurerm"", version = ""~> 3.0"" }
  }
}

resource ""azurerm_resource_group"" ""mlops"" {
  name     = ""mlops-${var.environment}-rg""
  location = var.location
}

resource ""azurerm_machine_learning_workspace"" ""ws"" {
  name                = ""mlops-${var.environment}-ws""
  location            = azurerm_resource_group.mlops.location
  resource_group_name = azurerm_resource_group.mlops.name
  identity { type = ""SystemAssigned"" }
}

resource ""azurerm_kubernetes_cluster"" ""aks"" {
  name                = ""mlops-aks""
  location            = azurerm_resource_group.mlops.location
  resource_group_name = azurerm_resource_group.mlops.name
  dns_prefix          = ""mlops""

  default_node_pool {
    name       = ""system""
    node_count = 2
    vm_size    = ""Standard_DS2_v2""
  }

  identity { type = ""SystemAssigned"" }
}
```

## .NET Application Insights Integration

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry(
    builder.Configuration[""ApplicationInsights:ConnectionString""]
);

app.MapPost(""/api/predict"", async (PredictRequest req,
    TelemetryClient telemetry, OnnxService svc) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    try
    {
        var result = svc.Predict(req.Features);
        telemetry.TrackEvent(""ModelPrediction"",
            new Dictionary<string, string>  { [""Version""] = ""1.0.0"" },
            new Dictionary<string, double>  { [""LatencyMs""] = sw.ElapsedMilliseconds });
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        telemetry.TrackException(ex);
        return Results.StatusCode(500);
    }
});
```

## Congratulations! You have completed the MLOps Roadmap!

You now have practical knowledge of:

- Python fundamentals from a .NET perspective
- Core ML with scikit-learn and PyTorch
- MLOps tools: MLflow, DVC, experiment tracking
- Data engineering and pipeline automation
- Model training with hyperparameter optimization
- Model serving in both Python (FastAPI) and .NET (ONNX)
- CI/CD for ML with GitHub Actions
- Production monitoring and drift detection
- Cloud infrastructure with Kubernetes and Azure ML

**Keep building! Contribute to open source and practice with real projects.**
";

}
