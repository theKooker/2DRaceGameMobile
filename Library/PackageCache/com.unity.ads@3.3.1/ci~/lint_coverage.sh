#!/usr/bin/env bash

select_folder() {
    PROJECT_DIR="ci~/Development/"
    echo "Project directory set to $PROJECT_DIR"
}

select_unity() {
  echo "Select a Unity version"
  unitys=$(ls /Applications/Unity/Hub/Editor/ | tr '\t' '\n')
  select unity in ${unitys[@]}
  do
    if [[ -n "$unity" ]]; then
      UNITY_EXECUTABLE="/Applications/Unity/Hub/Editor/$unity/Unity.app/Contents/MacOS/Unity"
      echo "Unity Executable is $UNITY_EXECUTABLE"
      break
    else
      echo "Invalid option $$REPLY"
    fi
  done
}

select_platform() {
  echo "Select a platform"
  platforms=(
    "playmode"
    "editmode"
  )
  select platform in "${platforms[@]}"
  do
    if [[ -n "$platform" ]]; then
      TEST_PLATFORM=$platform
      echo "Platform is $TEST_PLATFORM"
      break
    else
      echo "Invalid option $$REPLY"
    fi
  done
}

if [[ "$1" == "-i" ]]; then
  echo "Running in interactive mode"
  select_folder
  select_unity
  select_platform
fi

if [[ -z "$PROJECT_DIR" || -z "$TEST_PLATFORM" ]]; then
  echo "Don't know what to test, since no variables are set. Try running $0 -i"
  exit 1
fi

# Retrieves the directory of this script. Used for accessing files local to it.
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
echo "Collecting tests coverage for $TEST_PLATFORM for project $PROJECT_DIR"

# Run tests.
Unity() {
    if [[ -n $UNITY_EXECUTABLE ]]; then
        $UNITY_EXECUTABLE $@
    else
        xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' /opt/Unity/Editor/Unity $@
    fi
}

RESULTS_FILE_NUNIT=$(pwd)/$PROJECT_DIR$TEST_PLATFORM-results-nunit.xml

Unity \
  -logFile \
  -nographics \
  -projectPath $(pwd)/$PROJECT_DIR \
  -runTests \
  -testPlatform $TEST_PLATFORM \
  -testResults $RESULTS_FILE_NUNIT \
  -enableCodeCoverage \
  -coverageResultsPath $(pwd)/$PROJECT_DIR$TEST_PLATFORM \
  -coverageOptions "generateHtmlReport;generateBadgeReport;assemblyFilters:+UnityEngine.Advertisements,+UnityEngine.Monetization" \
  -batchmode

rm $RESULTS_FILE_NUNIT

# GitLab succeeds or fails depending on the exit code of the last script ran.
# This script exits with the exit code from Unity.
UNITY_EXIT_CODE=$?

if [ $UNITY_EXIT_CODE -eq 0 ]; then
  echo "Run succeeded, no failures occurred";
elif [ $UNITY_EXIT_CODE -eq 2 ]; then
  echo "Run succeeded, some tests failed";
elif [ $UNITY_EXIT_CODE -eq 3 ]; then
  echo "Run failure (other failure)";
else
  echo "Unexpected exit code $UNITY_EXIT_CODE";
fi


coverage=$(cat ci~/Development/playmode/Report/Summary.xml | grep Linecoverage | sed -ne '/Linecoverage/{s/.*<Linecoverage>\(.*\)<\/Linecoverage>.*/\1/p;q;}')

  echo "-----------------CODE COVERAGE--------------------"
  echo "$coverage %"
  echo "Report $(pwd)/$PROJECT_DIR/playmode/Report/index.htm"
  echo "--------------------------------------------------"

if ! git diff-index --quiet HEAD --; then
    echo "Code style issue in following files:"
    git diff --name-only
    exit 1
fi

exit $UNITY_EXIT_CODE
